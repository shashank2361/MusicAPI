using Moq;
using MyMusic.Core.Repositories;
using MyMusic.Services;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;

namespace MyMusic.Test
{
    public class MusicServiceMoqTest
    {


        private readonly MusicService _musicService;
        private readonly Mock<IUnitOfWork> _uowMock = new Mock<IUnitOfWork>() ;


        public MusicServiceMoqTest()
        {
            _musicService = new MusicService(_uowMock.Object);

        }


        [Fact]
        public async Task  MusicService_GetMusicById_ReturnMusic()
        {


            //Arrange
            var id = It.IsAny<int>();
            var listMusic = new List<MyMusic.Core.Model.Music>();

            var musicTest = new MyMusic.Core.Model.Music()
            {
                Id=id,
                Name = "Music Name",

                ArtistId = 1,
                Artist = new Core.Model.Artist()
                {
                    Id = 1,
                    Name = "ArtistName",
                    Musics = listMusic 
                }
            };

            _uowMock.Setup(x => x.Musics.GetWithArtistByIdAsync (id) ).ReturnsAsync(musicTest);
            //ACT
            var music = await _musicService.GetMusicById(id);

            //Assert
            Assert.Equal(id, music.Id);
            Assert.IsType<MyMusic.Core.Model.Music>(music);


        }

        [Fact]
        public async Task MusicService_GetMusicById_GetAllWithArtist()
        {


            //Arrange
            var id = It.IsAny<int>();
            var listMusic = new List<MyMusic.Core.Model.Music>();

            var musicTest = new List<MyMusic.Core.Model.Music>()
            {
              new MyMusic.Core.Model.Music(){    Id = 1,
                Name = "Music Name1",

                ArtistId = 1,
                Artist = new Core.Model.Artist()
                {
                    Id = 1,
                    Name = "ArtistName",
                    Musics = listMusic
                }
              },

              new MyMusic.Core.Model.Music(){    Id = 2,
                Name = "Music Name2",

                ArtistId = 2,
                Artist = new Core.Model.Artist()
                {
                    Id = 2,
                    Name = "ArtistName",
                    Musics = listMusic
                }
              }
            };



            _uowMock.Setup(x => x.Musics.GetAllWithArtistAsync( )).ReturnsAsync(musicTest);
            //ACT
            var musics = await _musicService.GetAllWithArtist( );

            //Assert
            Assert.Equal(musicTest, musics);
            Assert.IsType<List<MyMusic.Core.Model.Music>>(musics);


        }


        [Fact]
        public async Task MusicService_CreateMusic_SaveSuccess()
        {

            //Arrange
            var id = 3;
            var listMusic = new List<MyMusic.Core.Model.Music>();

            


            var newMusic = new MyMusic.Core.Model.Music()
            {
                Id = id,
                Name = "Music Name3",

                ArtistId = 1,
                Artist = new Core.Model.Artist()
                {
                    Id = 1,
                    Name = "ArtistName3",
                    Musics = listMusic
                }
            };


            _uowMock.Setup(x => x.Musics.AddAsync(newMusic));
            var newId = _uowMock.Setup(x => x.CommitAsync()).ReturnsAsync(1);

            
            //ACT
            var musics = await _musicService.CreateMusic(newMusic);

            _uowMock.Setup(x => x.Musics.GetWithArtistByIdAsync(id)).ReturnsAsync(musics);
            //ACT
            var getnewmusic = await _musicService.GetMusicById(id);


            //Assert
            Assert.Equal(id, getnewmusic.Id);
            Assert.Equal(id, musics.Id);
            Assert.IsType<MyMusic.Core.Model.Music>(musics);


        }



    }
}
