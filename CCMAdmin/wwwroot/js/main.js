import Vue from 'vue';
import CcmLoader from './region-manager/ccm-loader.vue';
import CcmRegionDeleter from './region-manager/ccm-region-deleter.vue';
import CcmRegionEditor from './region-manager/ccm-region-editor.vue';
import CcmRegionConstructor from './region-manager/ccm-region-constructor.vue';
import CcmRegion from './region-manager/ccm-region.vue';

//new Vue({
//    el: '#_x',
//    components: {
//        CcmLoader,
//        CcmRegionDeleter
//    }
//})

new Vue({
    el: '#_regionList',
    components: {
        CcmLoader,
        CcmRegionDeleter,
        CcmRegionEditor,
        CcmRegionConstructor,
        CcmRegion
    },
    data: {
        regions: regions
    },
    methods: {
        onRegionCreated: function (region) {
            this.regions.push(region);
        },

        onRegionDeleted: function (d) {
            let f = function (x) {
                return x.id === d.deletedId;
            };

            let i = this.regions.findIndex(f);

            if (i >= 0) {
                this.regions.splice(i, 1);
            }

            this.regions = this.regions
                .concat(d.promotedRegions);
        }
    }
});