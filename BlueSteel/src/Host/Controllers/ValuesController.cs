using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using Microsoft.Extensions.Logging;
using BlueSteel.Host.Data;

namespace BlueSteel.Host.Controllers
{
    /// <summary>
    /// Defines the value controller.
    /// </summary>
    [Route("values")]
    public class ValuesController : Controller
    {
        #region Overhead

        /// <summary>
        /// Holds the logger.
        /// </summary>
        private ILogger<ValuesController> Logger
        {
            get;
            set;
        }

        /// <summary>
        /// Holds the value repository.
        /// </summary>
        private IValueRepository Repository
        {
            get;
            set;
        }

        /// <summary>
        /// Create an instance of the value controller.
        /// </summary>
        /// <param name="repo">The repo to use.</param>
        /// <param name="logger">The logger to use.</param>
        public ValuesController(IValueRepository repo, ILogger<ValuesController> logger)
        {
            this.Repository = repo;
            this.Logger = logger;
        }

        /// <summary>
        /// Process the id, if it is not in the collection return the provided error code.
        /// </summary>
        /// <param name="id">The id to process.</param>
        /// <param name="processor">The post processing step.</param>
        /// <param name="code">The code to provide on failures.</param>
        private void Process(int id, Action<int, string> processor, HttpStatusCode code = HttpStatusCode.NotFound)
        {
            if (this.Repository.Contains(id))
            {
                processor(id, this.Repository[id]);
            }
            else
            {
                Logger.LogInformation($"No such id: {id}");
                StatusCode((int) code);
            }
        }

        #endregion

        /// <summary>
        /// Get the values.
        /// </summary>
        /// <returns>Returns the values.</returns>
        [HttpGet]
        public IEnumerable<string> Get()
        {
            return this.Repository.Values;
        }

        /// <summary>
        /// Get the value of an id.
        /// </summary>
        /// <param name="id">The id to get.</param>
        /// <returns>Returns the value or null if it wasn't found.</returns>
        [HttpGet("{id}")]
        public string Get(int id)
        {
            string output = null;
            Process(id, (index, val) => { output = val; });
            return output;
        }

        /// <summary>
        /// Add a value to the collection.
        /// </summary>
        /// <param name="value">The value to add.</param>
        /// <returns>Returns the id of the value created.</returns>
        [HttpPost]
        public int Post([FromBody]string value)
        {
            return this.Repository.Add(value);
        }

        /// <summary>
        /// Update the value of the id specified.
        /// </summary>
        /// <param name="id">The id to update.</param>
        /// <param name="value">The value to save.</param>
        [HttpPut("{id}")]
        public void Put(int id, [FromBody]string value)
        {
            Process(id, (index, val) => { this.Repository[index] = value; });
        }

        /// <summary>
        /// Remove the value.
        /// </summary>
        /// <param name="id">The index to delete.</param>
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
            Process(id, (index, val) => { this.Repository.Remove(index); });
        }
    }
}
