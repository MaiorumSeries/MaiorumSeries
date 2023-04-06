using MaiorumSeries.GedComModel;
using System;
using System.Collections.Generic;
using System.Text;

namespace MaiorumSeries.GedComLogic
{
    /// <summary>
    /// The relation shio structure is a strongly types structure of the relation ship to the proband
    /// </summary>
    public class ReleationShipStructure
    {
        ReleationshipIndividual _proband;

        public ReleationShipStructure(ReleationshipIndividual individual)
        {
            _proband = individual;
        }

        public ReleationshipIndividual Proband { get
            {
                return _proband;
            }
        }
        public Dictionary<int, Generation> Generations { get; set; } = new Dictionary<int, Generation>();

        public List<IndividualRecord> TribeCandidates { get; set; } = new List<IndividualRecord>();

        private void Build(Model model, ReleationshipIndividual from, int generation)
        {
            IndividualRecord father = null;
            IndividualRecord mother = null;
            bool isTribeCandidate = false;

            from.Individual.DirectAncestor = true;
            if (from.Individual.ChildFrom != null)
            {
                var family = model.Families.Find(x => x.XrefId == from.Individual.ChildFrom.Value);

                if (family != null)
                {
                    if (family.Husband == null && family.Wife == null)
                    {
                        isTribeCandidate = true;
                    }
                    if (family.Husband != null)
                    {
                        father = model.Individuals.Find(x => x.XrefId == family.Husband.Value);
                    }
                    if (family.Wife != null)
                    {
                        mother = model.Individuals.Find(x => x.XrefId == family.Wife.Value);
                    }

                    if (father != null)
                    {
                        var fatherRecord = from.GetAsFather(father);
                        AddInGeneration(generation + 1, fatherRecord);
                        Build(model, fatherRecord, generation + 1);
                    }
                    if (mother != null)
                    {
                        var motherRecord = from.GetAsMother(mother);
                        AddInGeneration(generation + 1, motherRecord);
                        Build(model, motherRecord, generation + 1);
                    }
                }
 
            }
            else
            {
                isTribeCandidate = true;
            }
     

            if (isTribeCandidate)
            {
                if (from.Individual.Sex == "M" && generation > 10)
                {
                    TribeCandidates.Add(from.Individual);
                }

            }
        }

        private void AddInGeneration(int generation, ReleationshipIndividual record)
        {
            if (!Generations.ContainsKey (generation))
            {
                Generations.Add(generation, new Generation());
            }
            Generations[generation].Add(record);
        }

        public void Build (Model model)
        {
            Build(model, Proband, 0);

        }
    }
}
