using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Zenith.Models;

namespace Zenith.Assets.Utils
{
    public class MapperUtil
    {
        public static Mapper Mapper = new Mapper(new MapperConfiguration(cfg =>
        {
            cfg.CreateMap<Company, Company>();
            cfg.CreateMap<Machine, Machine>();
            cfg.CreateMap<Material, Material>();
            cfg.CreateMap<Note, Note>();
            cfg.CreateMap<Person, Person>();
        }));
    }
}
