using Microsoft.AspNetCore.Mvc;
using NHibernate.Criterion;
using PycApi.Bussiness;
using PycApi.Context;
using PycApi.Model;
using Serilog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PycApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ContainerController : ControllerBase
    {
        private readonly IMapperSession<Container> containerSession;
        private readonly IMapperSession<Vehicle> vehicleSession;

        public ContainerController(IMapperSession<Container> containerSession, IMapperSession<Vehicle> vehicleSession)
        {
            this.containerSession = containerSession;
            this.vehicleSession = vehicleSession;
             
        }

        [HttpGet("{vehicleId}/{clusterCount}")]
        public ActionResult<List<List<ContainersWithCluster>>> Get(long vehicleId, int clusterCount)
        {
            
            var  vehicle =  vehicleSession.entity.Where(c => c.Id == vehicleId).FirstOrDefault(); // girilen  araç Id'sine araç alınır.
            if (vehicle == null) return NotFound(); // Id boşsa 404 döner.

            var containers = containerSession.entity.Where(c => c.VehicleId == vehicleId).ToList(); // araca ait konteynırlar listelenir.
          
            var list = new List<ContainersWithCluster>();
            for (int i = 0; i < containers.Count - 1; i++) // mesafe hesabı için for döngüsü ile işlem başlatılır.
                list.Add(Calculation.GetCluster(containers[i], containers[i + 1])); // sonuç  uzaklıkla  beraber eklenir.

            var clusteredList = Calculation.Cluster(list, (int)clusterCount); // algoritma çağrılır ve belirtilen küme sayısı kadar kümelenir.
            return Ok(clusteredList); // sonuç alıcıya döndürülür.
        }

    }
}
