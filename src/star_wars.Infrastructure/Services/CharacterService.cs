using AutoMapper;
using star_wars.Application.Common.Interfaces.Repositories;
using star_wars.Application.Common.Interfaces.Services;
using star_wars.Application.Common.Models.Dto.Character;
using star_wars.Application.Common.Models.ViewModels.Character;
using star_wars.Core.Entities;

namespace star_wars.Infrastructure.Services;

public class CharacterService : ICharacterService, ICharacterViewModelService
{
    private readonly ICharacterRepository _characterRepository;
    private readonly IMapper _mapper;

    public CharacterService(
        ICharacterRepository characterRepository,
        IMapper mapper)
    {
        _characterRepository = characterRepository;
        _mapper = mapper;
    }

    public async Task<ICollection<GetCharacterDto>> GetAllCharactersAsync()
    {
        var entityList = await _characterRepository.GetAllCharactersAsync();

        return _mapper.Map<ICollection<GetCharacterDto>>(entityList);
    }

    public async Task<GetCharacterDto> GetCharacterByIdAsync(int id)
    {
        var entity = await _characterRepository.GetCharacterByIdAsync(id);
        if (entity == null)
        {
            throw new Exception();
        }
        
        return _mapper.Map<GetCharacterDto>(entity);
    }

    public async Task<GetCharacterDto> AddCharacterAsync(AddCharacterDto newCharacter)
    {
        var entity = _mapper.Map<Character>(newCharacter);
        await _characterRepository.AddCharacterAsync(entity);
        return _mapper.Map<GetCharacterDto>(entity);
    }

    public async Task<GetCharacterDto> UpdateCharacterAsync(UpdateCharacterDto updateCharacter)
    {
        var entity = _mapper.Map<Character>(updateCharacter);
        await _characterRepository.UpdateCharacterAsync(entity);
        return _mapper.Map<GetCharacterDto>(entity);
    }

    public async Task<bool> DeleteCharacterByIdAsync(int id)
    {
        return await _characterRepository.DeleteCharacterByIdAsync(id);
    }
    

    
    
    public CharacterListViewModel MapCharacterDtoListToViewModel(ICollection<GetCharacterDto> charactersDto)
    {
        var characterListViewModel = _mapper.Map<CharacterListViewModel>(charactersDto);

        return characterListViewModel;
    }

    public CharacterViewModel MapCharacterDtoToViewModel(GetCharacterDto characterDto)
    {
        var characterViewModel = _mapper.Map<CharacterViewModel>(characterDto);

        return characterViewModel;
    }

    public UpdateCharacterDto MapViewModelToUpdateCharacterDto(CharacterViewModel characterViewModel)
    {
        var characterDto = _mapper.Map<UpdateCharacterDto>(characterViewModel);

        return characterDto;
    }
}