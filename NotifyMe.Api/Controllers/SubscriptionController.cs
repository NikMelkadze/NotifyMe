using Microsoft.AspNetCore.Mvc;
using NotifyMe.Application.Contracts;

namespace NotifyMe.Api.Controllers;

public class SubscriptionController(ISubscriptionService subscriptionService) : Controller
{
    
}