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
            cfg.CreateMap<Buy, Buy>();
            cfg.CreateMap<BuyItem, BuyItem>();
            cfg.CreateMap<Cash, Cash>();
            cfg.CreateMap<Cheque, Cheque>();
            cfg.CreateMap<Company, Company>();
            cfg.CreateMap<Machine, Machine>();
            cfg.CreateMap<Material, Material>();
            cfg.CreateMap<Note, Note>();
            cfg.CreateMap<Person, Person>();
            cfg.CreateMap<Outgo, Outgo>();
            cfg.CreateMap<MachineOutgo, MachineOutgo>();
            cfg.CreateMap<OutgoCategory, OutgoCategory>();
            cfg.CreateMap<Sale, Sale>();
            cfg.CreateMap<SaleItem, SaleItem>();
            cfg.CreateMap<Site, Site>();

            cfg.CreateMap<Mixture, Material>()
                .ForMember(material => material.Name, opt => opt.MapFrom(mixture => mixture.DisplayName))
                .ForMember(material => material.IsMixed, opt => opt.MapFrom(mixture => true));
        }));
    }
}
