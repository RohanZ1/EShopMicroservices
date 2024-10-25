﻿using Basket.API.Models;
using Carter;
using Mapster;
using MediatR;

namespace Basket.API.Basket.GetBasket
{
    //public record GetBasketRequest(string username);
    public record GetBasketResponse(ShoppingCart Cart);
    public class GetBasketEndpoint : ICarterModule
    {
        public void AddRoutes(IEndpointRouteBuilder app)
        {
            app.MapGet("/basket/{userName}",
            async (string userName,ISender sender)=>{
                var result = await sender.Send(new GetBasketQuery(userName));
                var response=result.Adapt<GetBasketResponse>();
                return Results.Ok(response);
            });
           
        }
    }
}