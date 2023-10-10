using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Zenith.Assets.Values.Dtos;
using Zenith.Assets.Values.Enums;
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
            cfg.CreateMap<SalaryPayment, SalaryPayment>();
            cfg.CreateMap<SaleItem, SaleItem>();
            cfg.CreateMap<Site, Site>();

            cfg.CreateMap<Mixture, Material>()
                .ForMember(material => material.Name, opt => opt.MapFrom(mixture => mixture.DisplayName))
                .ForMember(material => material.IsMixed, opt => opt.MapFrom(mixture => true));

            cfg.CreateMap<Sale, Cash>()
                .ForMember(cash => cash.MoneyTransactionType, opt => opt.MapFrom(order => order.CashState == CashStates.Cash ? MoneyTransactionTypes.CashSale : MoneyTransactionTypes.NonCashSale))
                .ForMember(cash => cash.IssueDateTime, opt => opt.MapFrom(order => order.DateTime))
                .ForMember(cash => cash.RelatedEntityId, opt => opt.MapFrom(order => order.SaleId));

            cfg.CreateMap<Buy, Cash>()
                .ForMember(cash => cash.MoneyTransactionType, opt => opt.MapFrom(order => order.CashState == CashStates.Cash ? MoneyTransactionTypes.CahBuy : MoneyTransactionTypes.NonCahBuy))
                .ForMember(cash => cash.CostCenter, opt => opt.MapFrom(_ => CostCenters.Workshop))
                .ForMember(cash => cash.IssueDateTime, opt => opt.MapFrom(order => order.DateTime))
                .ForMember(cash => cash.Value, opt => opt.MapFrom(order => order.Items.Sum(bi => bi.TotalPrice)))
                .ForMember(cash => cash.RelatedEntityId, opt => opt.MapFrom(order => order.BuyId));

            cfg.CreateMap<Outgo, Cash>()
                .ForMember(cash => cash.MoneyTransactionType, opt => opt.MapFrom(order => order.CashState == CashStates.Cash ? MoneyTransactionTypes.CashOutgo : MoneyTransactionTypes.NonCashOutgo))
                .ForMember(cash => cash.CostCenter, opt => opt.MapFrom(order => order.OutgoType == OutgoTypes.Direct ? CostCenters.Workshop : CostCenters.Consumables))
                .ForMember(cash => cash.IssueDateTime, opt => opt.MapFrom(order => order.DateTime))
                .ForMember(cash => cash.Value, opt => opt.MapFrom(order => order.Value))
                .ForMember(cash => cash.RelatedEntityId, opt => opt.MapFrom(order => order.OutgoId));

            cfg.CreateMap<SalaryPayment, Cash>()
                .ForMember(cash => cash.MoneyTransactionType, opt => opt.MapFrom(order => order.CostCenter == CostCenters.Workshop ? MoneyTransactionTypes.WorkshopSalary : MoneyTransactionTypes.TransportaionSalary))
                .ForMember(cash => cash.IssueDateTime, opt => opt.MapFrom(order => order.DateTime))
                .ForMember(cash => cash.Value, opt => opt.MapFrom(order => order.PaidValue))
                .ForMember(cash => cash.RelatedEntityId, opt => opt.MapFrom(order => order.SalaryPaymentId));

            cfg.CreateMap<MachineOutgo, Cash>()
                .ForMember(cash => cash.MoneyTransactionType, opt => opt.MapFrom(order => order.CashState == CashStates.Cash ? MoneyTransactionTypes.CashMachineOutgo : MoneyTransactionTypes.NonCashMachineOutgo))
                .ForMember(cash => cash.CostCenter, opt => opt.MapFrom(_ => CostCenters.Transportation))
                .ForMember(cash => cash.IssueDateTime, opt => opt.MapFrom(order => order.DateTime))
                .ForMember(cash => cash.Value, opt => opt.MapFrom(order => order.Value))
                .ForMember(cash => cash.RelatedEntityId, opt => opt.MapFrom(order => order.OutgoId));

            cfg.CreateMap<SalaryStatisticsDto, SalaryStatisticsDto>();
        }));
    }
}
