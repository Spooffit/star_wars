﻿namespace star_wars.Core.Entities;

public class Character
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string OriginName { get; set; }
    public int Birthdate { get; set; }
    public Planet Planet { get; set; }
    public string Gender { get; set; }
    public string Species { get; set; }
    public float Height { get; set; }
    public string HairColor { get; set; }
    public string EyeColor { get; set; }
    public string Description { get; set; }
    public ICollection<Movie> Movies { get; set; }
}