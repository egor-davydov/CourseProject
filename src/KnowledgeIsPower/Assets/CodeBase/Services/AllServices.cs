using System;
using System.Collections.Generic;

namespace CodeBase.Services
{
  public class AllServices
  {
    private static AllServices _instance;
    public static AllServices Container => _instance ?? (_instance = new AllServices());

    private readonly Dictionary<Type, IService> _services;

    private AllServices() =>
      _services = new Dictionary<Type, IService>();

    public void RegisterSingle<TService>(TService service) where TService : IService =>
      _services.Add(typeof(TService), service);

    public TService Single<TService>() where TService : IService
    {
      return _services.TryGetValue(typeof(TService), out IService service)
        ? (TService)service
        : throw new ApplicationException(
          $"You try get access to unregistered service {typeof(TService).Name}. " +
          $"Register necessary service in BootstrapState");
    }
  }
}