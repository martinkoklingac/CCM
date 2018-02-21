<template>
    <div class="ccm-region" v-bind:class="{ 'child': isChild, 'root': !isChild, 'expanded': (isLoading || isExpanded)}">
        <div class="region-wrapper">
            <div class="info-section">
                <span>{{name}}</span>
            </div>
            <div class="action-section">
                <button v-on:click="onExpand"
                        v-if="isExpanded === false"
                        class="ccm-btn normal submit-btn">
                    Expand
                </button>
                <button v-on:click="onCollapse"
                        v-if="isExpanded"
                        class="ccm-btn normal submit-btn">
                    Collapse
                </button>

                <button v-on:click="onEdit"
                        :disabled="mode === 'Edit'"
                        class="ccm-btn normal submit-btn">
                    Edit
                </button>

                <button v-on:click="onDelete"
                        class="ccm-btn normal delete-btn">
                    Delete
                </button>
            </div>
        </div>

        <div v-if="mode === 'Delete'"
             class="editor-wrapper">
            <ccm-region-deleter v-bind:id='id'
                                v-bind:name='name'
                                v-bind:is-root='!isChild'
                                v-on:cancel="onCancelled"
                                v-on:deleted="onCurrentRegionDeleted" />
        </div>

        <div v-if="mode === 'Edit'">
            <ccm-region-editor v-bind:id='id'
                               v-on:cancel="onCancelled">
            </ccm-region-editor>
        </div>

        <div v-if="isExpanded"
             class="editor-wrapper">
            <ccm-region-constructor v-if="isExpanded"
                                    v-bind:parent-id='id'
                                    v-on:created="onRegionCreated" />
        </div>

        <div v-if="isExpanded"
             class="child-region-wrapper">
            <ccm-region v-for="region in childRegions" :key="region.id"
                        v-bind:id='region.id'
                        v-bind:parent-id='id'
                        v-bind:name='region.name'
                        v-on:deleted='onChildRegionDeleted' />
            <div v-if="childRegions.length == 0">
                There are no regions
            </div>
        </div>

        <div class="loader-wrapper"
             v-if="isLoading">
            <ccm-loader />
        </div>
    </div>
</template>

<script>
    import CcmLoader from './ccm-loader.vue';
    import CcmRegionConstructor from './ccm-region-constructor.vue';
    import CcmRegionEditor from './ccm-region-editor.vue';
    import CcmRegionDeleter from './ccm-region-deleter.vue';

    export default {
        name: "ccm-region",
        props: {
            id: {
                type: Number
            },
            parentId: {
                type: Number,
                default: null
            },
            name: {
                type: String,
                default: null
            }
        },
        components: {
            CcmLoader,
            CcmRegionConstructor,
            CcmRegionEditor,
            CcmRegionDeleter
        },
        methods: {
            onEdit: function () {
                this.mode = "Edit";
            },
            onCancelled: function () {
                this.mode = "None";
            },
            onExpand: function () {
                this.isLoading = true;

                $.get(window.location + "/get-children", { parentId: this.id }, $.proxy(function (d) {
                    this.childRegions = d;
                    this.isExpanded = true;
                    this.isLoading = false;
                }, this));
            },
            onCollapse: function () {
                this.isExpanded = false;
            },
            onDelete: function () {
                this.mode = "Delete";
            },

            /**
             *  Handles deleted event of ccm-region.
             *  Removes the deleted region from childRegions array and adds the promoted child regions
             *  to childRegions array.
             *
             *  @param {Object} d - Metadata that provides information about the deleted region
             *  @param {Number} d.deletedId - Id of the deleted region
             *  @param {Object[]} d.childRegions - List of child regions that will have to be merged up one level
             *  @param {Number} d.childRegions[].id - Child region id
             *  @param {String} d.childRegions[].name - Child region name
             */
            onChildRegionDeleted: function (d) {

                let f = function (x) {
                    return x.id === d.deletedId;
                };

                let i = this.childRegions.findIndex(f);

                if (i >= 0) {
                    this.childRegions.splice(i, 1);
                }

                this.childRegions = this.childRegions
                    .concat(d.promotedRegions);
            },

            /**
             *  Event handler that bubbles up deleted event of ccm-region-deleter component
             *
             *  @param {Object} d - Metadata that provides information about the deleted region
             *  @param {Number} d.deletedId - Id of the deleted region
             *  @param {Object[]} d.childRegions - List of child regions that will have to be merged up one level
             *  @param {Number} d.childRegions[].id - Child region id
             *  @param {String} d.childRegions[].name - Child region name
             */
            onCurrentRegionDeleted: function (d) {
                this.mode = "None";
                this.$emit("deleted", d);
            },

            onRegionCreated: function (d) {
                this.childRegions.push(d);
            }
        },

        created: function () {
            this.isChild = this.parentId != null;
        },
        data: function () {
            return {
                mode: 'None',
                isExpanded: false,
                isLoading: false,
                isChild: false,
                childRegions: null
            };
        }
    }
</script>