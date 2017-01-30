using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ProjectInformationManagementSystem.App_Start.Helper
{
    public class Report
    {
        public int numberOfProjects { get; set; }
        public int numberOfExtractionProjects { get; set; }
        public int numberOfLibraryPrepProjects { get; set; }
        public int numberOfCaptureProjects { get; set; }
        public int numberOfSequencingProjects { get; set; }
        public double totalCost { get; set; }
        public double totalCostCharged { get; set; }
        public double totalCostPending { get; set; }
        

    }
}