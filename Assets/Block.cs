using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Newtonsoft.Json;
using UnityEngine;

public class Block
{
    public enum Direction
    {
        Up = 0,
        North = 1,
        East = 2,
        West = 3,
        South = 4,
        Down = 5
    }

    private readonly string _materialName;

    public Vector3 ControlPoint;
    public bool IsControlPointLocked;
    public Vector3 Position;
    public int SearchMarker;


    public Block(ChunkObject parent, string materialName)
    {
        ControlPoint = new Vector3(.5f, .5f, .5f);

        Parent = parent;
        Position = new Vector3Int(0, 0, 0);
        SearchMarker = 0;
        IsControlPointLocked = false;
        _materialName = materialName;
    }


    public Block(ChunkObject parent, string materialName, Vector3 position)
    {
        ControlPoint = new Vector3(.5f, .5f, .5f);

        Parent = parent;
        Position = Vector3Int.FloorToInt(position);
        SearchMarker = 0;
        IsControlPointLocked = false;
        _materialName = materialName;
    }


    public Block(ChunkObject parent, string materialName, Vector3Int position)
    {
        ControlPoint = new Vector3(.5f, .5f, .5f);

        Parent = parent;
        Position = position;
        SearchMarker = 0;
        IsControlPointLocked = false;
        _materialName = materialName;
    }

    
    ~Block()
    {
        if (this.Adjoining.Down != null)
        {
            propagateDeletion(this.Adjoining.Down, this);
        }
        if (this.Adjoining.Up != null)
        {
            propagateDeletion(this.Adjoining.Up, this);
        }
        if (this.Adjoining.North != null)
        {
            propagateDeletion(this.Adjoining.North, this);
        }
        if (this.Adjoining.South != null)
        {
            propagateDeletion(this.Adjoining.South, this);
        }
        if (this.Adjoining.East != null)
        {
            propagateDeletion(this.Adjoining.East, this);
        }
        if (this.Adjoining.West != null)
        {
            propagateDeletion(this.Adjoining.West, this);
        }
    }

    public BlockFaces<Block> Adjoining { get; private set; }
    public BlockMaterial Material => BlockMaterial.GetMaterial(_materialName);
    public ChunkObject Parent { get; }
    public float Strength => BlockMaterial.GetMaterial(_materialName).Strength * 50f;


    public void AddAdjoining(Block block, Direction face)
    {
        switch (face)
        {
            case Direction.Up:
            {
                Adjoining.Up = block;
                break;
            }
            case Direction.North:
            {
                Adjoining.North = block;
                break;
            }
            case Direction.East:
            {
                Adjoining.East = block;
                break;
            }
            case Direction.West:
            {
                Adjoining.West = block;
                break;
            }
            case Direction.South:
            {
                Adjoining.South = block;
                break;
            }
            case Direction.Down:
            {
                Adjoining.Down = block;
                break;
            }
            default:
                throw new ArgumentOutOfRangeException(nameof(face), face, null);
        }
    }

    
    private void propagateDeletion(Block obj, Block sourceObj)
    {
        if (obj.Adjoining.Down == sourceObj)
        {
            obj.Adjoining.Down = null;
        }
        if (obj.Adjoining.Up == sourceObj)
        {
            obj.Adjoining.Up = null;
        }
        if (obj.Adjoining.North == sourceObj)
        {
            obj.Adjoining.North = null;
        }
        if (obj.Adjoining.South == sourceObj)
        {
            obj.Adjoining.South = null;
        }
        if (obj.Adjoining.East == sourceObj)
        {
            obj.Adjoining.East = null;
        }
        if (obj.Adjoining.West == sourceObj)
        {
            obj.Adjoining.West = null;
        }
    }

    
    [Serializable]
    public class BlockFaces<T>
    {
        private T _up;
        private T _north;
        private T _east;
        private T _west;
        private T _south;
        private T _down;

        [JsonProperty] 
        public T Up
        {
            get { return _up; }
            internal set { _up = value; }
        }

        [JsonProperty] 
        public T North        
        {
            get { return _north; }
            internal set { _north = value; }
        }
        
        [JsonProperty] 
        public T East
        {
            get { return _east; }
            internal set { _east = value; }
        }
        
        [JsonProperty] 
        public T West
        {
            get { return _west; }
            internal set { _west = value; }
        }
        
        [JsonProperty] 
        public T South
        {
            get { return _south; }
            internal set { _south = value; }
        }
        
        [JsonProperty] 
        public T Down
        {
            get { return _down; }
            internal set { _down = value; }
        }
    }


    [JsonObject(Title = "MaterialType")]
    public class BlockMaterial
    {
        [JsonIgnore] private static List<BlockMaterial> _materials;
        [JsonIgnore] private static string _blockTypesConfigFile;
        [JsonIgnore] private readonly FileSystemWatcher _fileWatcher;
        [JsonIgnore] private BlockFaces<MaterialIndex> _blockFaces;
        [JsonIgnore] private string _imagePath;
        [JsonIgnore] private string _name;
        [JsonIgnore] private int _blockiness;
        [JsonIgnore] private int _density;
        [JsonIgnore] private int _occlusion;
        [JsonIgnore] private int _strength;
        [JsonIgnore] private bool _isSolid;


        private BlockMaterial()
        {
            //_fileWatcher = new FileSystemWatcher(ImageFile.FullName);
            //_fileWatcher.Changed += FileWatcherOnChanged;
        }
       
        
        [JsonIgnore] private Vector2[] _faceTextureLocations { get; set; }
        [JsonIgnore] public FileInfo ImageFile { get; private set; }
        
        [JsonProperty(Order = 2, Required = Required.Always)]
        private string ImagePath
        {
            get { return _imagePath; }
            set { _imagePath = value; }
        }    
        
        [JsonProperty(Order = 3, Required = Required.Always)]
        public BlockFaces<MaterialIndex> Faces
        {
            get { return _blockFaces; }
            private set { _blockFaces = value; }
        }
        
        [JsonProperty(Order = 1, Required = Required.Always)]
        public string Name
        {
            get { return _name; }
            private set { _name = value; }
        }
        
        [JsonProperty(Order = 4, Required = Required.Always)]
        public int Blockiness
        {
            get { return _blockiness; }
            private set { _blockiness = value; }
        }
        
        [JsonProperty(Order = 5, Required = Required.Always)]
        public int Density
        {
            get { return _density; }
            private set { _density = value; }
        }
        
        [JsonProperty(Order = 6, Required = Required.Always)]
        public int Occlusion
        {
            get { return _occlusion; }
            private set { _occlusion = value; }
        }
        
        [JsonProperty(Order = 7, Required = Required.Always)]
        public int Strength
        {
            get { return _strength; }
            private set { _strength = value; }
        }
        
        [JsonProperty(Order = 8, Required = Required.Always)]
        public bool IsSolid
        {
            get { return _isSolid; }
            private set { _isSolid = value; }
        }


        private void FileWatcherOnChanged(object sender, FileSystemEventArgs e)
        {
            LoadMaterialsFromFile(_blockTypesConfigFile);
        }


        public static void LoadMaterialsFromFile(string path)
        {
            // Loads data provided by developer into the materials array
            _blockTypesConfigFile = path;

            if (!File.Exists(path))
            {
                throw new FileNotFoundException($"File provided to Block.BlockMaterial.LoadMaterialFromFile(string path) does not exist! (path: {path})");
            }
           
            string jsonData = File.ReadAllText(path);
            _materials = JsonConvert.DeserializeObject<List<BlockMaterial>>(jsonData);
        }


        public static BlockMaterial GetMaterial(string name)
        {
            var result = _materials.First(x => x.Name == name);

            if (result == null) throw new NullReferenceException($"Material name {name} was not found!");

            return result;
        }


        public static BlockMaterial GetMaterial(Block block)
        {
            if (block.Material == null)
                throw new NullReferenceException($"Material name {block._materialName} was not found!");

            return block.Material;
        }


        public static Vector2[][] GetAllMaterialData()
        {
            var materialData = new Vector2[_materials.Count][];

            for (var i = 0; i < _materials.Count; ++i) materialData[i] = _materials[i]._faceTextureLocations;

            return materialData;
        }


        public static int Count()
        {
            return _materials.Count;
        }

        [Serializable]
        public struct MaterialIndex
        {
            [JsonProperty(Required = Required.Always)]
            public int RowIndex;
            
            [JsonProperty(Required = Required.Always)]
            public int ColumnIndex;
        }
    }
}