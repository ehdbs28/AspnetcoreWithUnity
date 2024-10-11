using System.ComponentModel.DataAnnotations.Schema;
using System.Numerics;

namespace Backend.DB.Models;

public class Character
{
    public int OwnerUserId { get; set; }
    public int Level { get; set; }
    
    public float LastPositionX { get; set; }
    public float LastPositionY { get; set; }
    public float LastPositionZ { get; set; }

    [NotMapped]
    public Vector3 LastPosition
    {
        get
        {
            return new Vector3(LastPositionX, LastPositionY, LastPositionZ);
        }
        set
        {
            LastPositionX = value.X;
            LastPositionY = value.Y;
            LastPositionZ = value.Z;
        }
    }
}