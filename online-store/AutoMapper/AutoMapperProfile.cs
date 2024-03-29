﻿using AutoMapper;

namespace online_store.AutoMapper
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            //<sourse , destention>
            CreateMap<User, CustomerDTO>();
            CreateMap<CustomerDTO, User>();

            CreateMap<CustomerDTO, Address>();
            CreateMap<Address, CustomerDTO>();

            
            CreateMap<Category, CategoryDTO>();
            CreateMap<Category, CategoryReadDTO>();

            CreateMap<CategoryDTO , Category>();

            CreateMap<CategoryReadDTO, Category>();

            CreateMap<ProductWriteDto, Product>();
            CreateMap<Product, ProductWriteDto>();

  

            CreateMap<Address, CheckoutDto>();
            CreateMap<CheckoutDto, Address>();


        }
    }
}
