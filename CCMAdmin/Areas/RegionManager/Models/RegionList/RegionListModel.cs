using System.Collections;
using System.Collections.Generic;

namespace CCMAdmin.Areas.RegionManager.Models.RegionList
{
    public class RegionListModel :
        IReadOnlyCollection<Region>
    {
        #region CONSTRUCTORS
        public RegionListModel(
            IReadOnlyCollection<Region> regions)
        {
            this.Regions = regions;
        }
        #endregion


        #region PUBLIC PROPERTIES
        public int Count => this.Regions.Count;
        #endregion

        #region PRIVATE PROPERTIES
        private IReadOnlyCollection<Region> Regions { get; }
        #endregion

        #region PUBLIC METHODS
        public IEnumerator<Region> GetEnumerator() => this.Regions.GetEnumerator();
        IEnumerator IEnumerable.GetEnumerator() => this.Regions.GetEnumerator();
        #endregion
    }
}
