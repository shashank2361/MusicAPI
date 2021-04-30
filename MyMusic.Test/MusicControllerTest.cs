using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Moq;
using MyMusic.Api.Controllers;
using MyMusic.Api.Mapping;
using MyMusic.Api.Resources;
using MyMusic.Core.Repositories;
using MyMusic.Core.Services;
using MyMusic.Data;
using MyMusic.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;


namespace MyMusic.Test
{


     




    public class MusicControllerTest
    {

        MusicsController _controller;
        IMusicService _musicService;
        IMapper _mapper;
        Mock<MyMusicDbContext> mockDBContext = new Mock<MyMusicDbContext>();
        Mock<IMapper> mockMapper = new Mock<IMapper>();
        IUnitOfWork _UnitOfWork;



        public MusicControllerTest()
        {
              _mapper = new MapperConfiguration(cfg => cfg.AddProfile( new MappingProfile())).CreateMapper();
     
        }
        [Fact]
        public async Task MusicsController_GetMusicById_ReturnMusic()
        {
            var options = new DbContextOptionsBuilder<MyMusicDbContext>()
                    .UseInMemoryDatabase(databaseName: "TestNewListDb").Options;

            using (var context = new MyMusicDbContext(options))
            {
                _UnitOfWork = new UnitOfWork(context); 
                _musicService = new MusicService(_UnitOfWork);
               var  controller = new MusicsController(_musicService, _mapper);


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

              await  context.Musics.AddRangeAsync(musicTest);
              await context.SaveChangesAsync();
              var result =   controller.GetAllMusics().GetAwaiter().GetResult().Result as  OkObjectResult;
              Assert.IsType<OkObjectResult>(result    );
              // Assert.Equal(musicTest , result.Value );



            };


 

        }


        [Fact]
        public async Task MusicsController_GetMusicById_ReturnOk()
        {
            var options = new DbContextOptionsBuilder<MyMusicDbContext>()
                    .UseInMemoryDatabase(databaseName: "TestNewListDb").Options;

            using (var context = new MyMusicDbContext(options))
            {
                _UnitOfWork = new UnitOfWork(context);
                _musicService = new MusicService(_UnitOfWork);
                var controller = new MusicsController(_musicService, _mapper);


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

                await context.Musics.AddRangeAsync(musicTest);
                await context.SaveChangesAsync();
                var result = controller.GetMusicById(1).GetAwaiter().GetResult().Result as OkObjectResult;
                Assert.IsType<OkObjectResult>(result);
                var musicResourse = Assert.IsType<MusicResource>(result.Value);

                Assert.Equal(1 , musicResourse.Id );
                Assert.Equal("Music Name1" , musicResourse.Name );

            };

        }

        [Fact]
        public async Task MusicsController_CreateMusic_ReturnCreated()
        {
            var options = new DbContextOptionsBuilder<MyMusicDbContext>()
                    .UseInMemoryDatabase(databaseName: "TestNewListDb").Options;
            IActionResult result;
            using (var context = new MyMusicDbContext(options))
            {
                _UnitOfWork = new UnitOfWork(context);
                _musicService = new MusicService(_UnitOfWork);


 
                var saveMusicResource  =     new SaveMusicResource (){   
                    Name = "Music Name1",
                    ArtistId = 1,
                };
                var controller = new MusicsController(_musicService, _mapper);

                

                
                  result = controller.CreateMusic(saveMusicResource)  as  IActionResult;

                 var createResult = Assert.IsType<CreatedAtActionResult>(result);
                var musicResourse = Assert.IsType<MusicResource>(createResult.Value);

                Assert.IsType<CreatedAtActionResult>(createResult);
                Assert.IsType<MusicResource>(musicResourse);
                 


                 Assert.Equal(1, musicResourse.Id);
                Assert.Equal("Music Name1", musicResourse.Name);

            };

        }
    }


    






}