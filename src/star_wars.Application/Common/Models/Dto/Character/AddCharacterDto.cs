using star_wars.Core.Entities;

namespace star_wars.Application.Common.Models.Dto.Character;

public class AddCharacterDto
{
    public string Name { get; set; }
    public string OriginName { get; set;  }
    public int Birthdate { get; set;  }
    public Planet Planet { get; set;  }
    public string Gender { get; set;  }
    public string Species { get; set;  }
    public float Height { get; set;  }
    public string HairColor { get; set;  }
    public string EyeColor { get; set;  }
    public string Description { get; set;  }
    public ICollection<Movie> Movies { get; set;  }
}