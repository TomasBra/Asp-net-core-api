using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using Ukol_test.Models;

namespace Ukol_test.Controllers
{
    [Route("api")]
    [ApiController]
    public class ServicesController : ControllerBase
    {
        private ukolContext db = new ukolContext();
        private readonly ILogger<ServicesController> _logger;

        public ServicesController(ILogger<ServicesController> logger)
        {
            _logger = logger;
        }


        [HttpGet]
        [Route("services/{page}/{take}")]
        public List<Service> GetSomeServices(int page, int take)
        {
            _logger.LogInformation("Data from table 'Services' were taken from database.");
            //předpokládám, že je 5 záznamů na stránce
            return (db.Services.Include(x => x.ServiceType).Select(x => new Service { Id = x.Id, Name = x.Name, ServiceType = x.ServiceType, SpecificationTextRequired = x.SpecificationTextRequired }).Skip(page * 5).Take(take).ToList());
        }

        [HttpGet]
        [Route("services")]
        public List<Service> GetServices()
        {
            _logger.LogInformation("Data from table 'Services' were taken from database.");
            return (db.Services.Include(x => x.ServiceType).Select(x => new Service { Id = x.Id, Name = x.Name, ServiceType = x.ServiceType, SpecificationTextRequired = x.SpecificationTextRequired }).ToList());
        }

        [HttpPost]
        [Route("services/new")]
        public List<Service> AddService(Service service)
        {
            //jediná nenulovatelná položka(vyjma id)
            if (service.Name != null)
            {
                try
                {
                    db.Services.Add(service);
                    db.SaveChanges();
                }
                catch (Exception ex)
                {
                    _logger.LogError("Adding new service failed, " +ex.ToString());
                }
            }
            else
            {
                _logger.LogWarning("The property 'Name' is empty.");
            }
            return db.Services.Include(x => x.ServiceType).Select(x => new Service { Id = x.Id, Name = x.Name, ServiceType = x.ServiceType, SpecificationTextRequired = x.SpecificationTextRequired }).ToList();
        }

        [HttpDelete]
        [Route("services/delete/{delete_id}")]
        public List<Service> DeleteService(int delete_id)
        {
            if (delete_id != null)
            {
                try
                {
                    db.Services.Remove(db.Services.First(x => x.Id == delete_id));
                }
                catch (Exception ex)
                {
                    _logger.LogError("Removing service failed, " + ex.ToString());
                }
            }
            return db.Services.Include(x => x.ServiceType).Select(x => new Service { Id = x.Id, Name = x.Name, ServiceType = x.ServiceType, SpecificationTextRequired = x.SpecificationTextRequired }).ToList();
        }

        [HttpGet]
        [Route("servicetypes")]
        public List<ServiceType> GetServiceTypes()
        {
            _logger.LogInformation("Data from table 'ServiceTypes' were taken from database.");
            return (db.ServiceTypes.Include(x => x.Services).Select(x=>new ServiceType { Id = x.Id, Name = x.Name, Services = x.Services}).ToList());
        }

    }
}