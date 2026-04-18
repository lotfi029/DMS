global using Domain.Errors;
global using Domain.Services;
global using Domain.Entities;
global using Domain.Constants;
global using Domain.IRepositories;

global using Infrastructure.Services;
global using Infrastructure.Persistence;
global using Infrastructure.Repositories;
global using Infrastructure.Persistence.Seeders;
global using Infrastructure.Services.Authentication;


global using Application.DTOs.Users;
global using Application.DTOs.Auths;
global using Application.Interfaces;

global using SharedKernel;

global using Microsoft.AspNetCore.Authentication.JwtBearer;
global using System.Linq.Expressions;
global using Microsoft.Extensions.DependencyInjection;
global using Microsoft.Extensions.Configuration;
global using Microsoft.IdentityModel.Tokens;
global using Microsoft.AspNetCore.Identity;
global using Microsoft.AspNetCore.Http;
global using System.Security.Claims;
global using Microsoft.EntityFrameworkCore;
global using Microsoft.Extensions.Logging;
global using System.Text;
global using Mapster;