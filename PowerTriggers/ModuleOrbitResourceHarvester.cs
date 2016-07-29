
namespace PowerTriggers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using UnityEngine;

    public class ModuleOrbitResourceHarvester : ModuleResourceHarvester
    {
        private int counter = 0;

        protected override ConversionRecipe PrepareRecipe(double deltaTime)
        {
            if (HighLogic.LoadedSceneIsFlight && this.HarvesterType == 3)
            {
                // Handle resource generation here. 
                var request = new AbundanceRequest
                {
                    Altitude = this.vessel.altitude,
                    BodyId = FlightGlobals.currentMainBody.flightGlobalsIndex,
                    CheckForLock = false,
                    // BiomeName = ??
                    Latitude = this.vessel.latitude,
                    Longitude = this.vessel.longitude,
                    ResourceType = HarvestTypes.Exospheric,
                    ResourceName = this.ResourceName
                };

                var abundance = ResourceMap.Instance.GetAbundance(request);
                if(abundance < 1E-09 || abundance < this.HarvestThreshold)
                {
                    this.status = "missing resource";
                    this.IsActivated = false;
                    return null;
                } else
                {
                    // we found a resource!
                    if (counter % 10 == 0)
                    {
                        ScreenMessages.PostScreenMessage("Found " + this.ResourceName);
                    }

                    this.status = "harvesting";
                    var conversionRate = abundance * this.Efficiency * this.GetCrewBonus();
                    var orbitalSpeed = this.vessel.orbit.vel.magnitude + 1;
                    var intakeRate = conversionRate * orbitalSpeed;

                    return GenerateRecipie(intakeRate);
                }
            }
            else
            {
                return base.PrepareRecipe(deltaTime);
            }
        }

        private ConversionRecipe GenerateRecipie(double intakeRate)
        {
            var ret = new ConversionRecipe();
            try
            {
                ret.Inputs.AddRange(this.inputList);
                ret.Outputs.Add(new ResourceRatio
                {
                    ResourceName = this.ResourceName, 
                    Ratio = intakeRate,
                    DumpExcess = false
                });
            }
            catch (Exception ex)
            {
                MonoBehaviour.print(ex);
            }

            return ret;
        }
    }
}
