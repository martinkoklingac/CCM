<template>
    <div class="ccm-region-constructor">
        <div class="action-container"
             v-if="!isLoading">
            <button v-if="!isFormVisible"
                    v-on:click="onAddNew"
                    class="ccm-btn normal submit-btn">
                Add New
            </button>
            <button v-if="isFormVisible"
                    v-on:click="onCancel"
                    class="ccm-btn normal cancel-btn">
                Cancel
            </button>
            <button v-if="isFormVisible"
                    v-on:click="onAdd"
                    class="ccm-btn normal submit-btn">
                Add
            </button>
        </div>

        <div class="form-container"
             v-if="isFormVisible && !isLoading">
            <div class="ccm-input-container">
                <label>Region Name</label>
                <input type="text" class="ccm-input normal"
                       v-model.trim="model.name"
                       v-bind:class="{'error':isModelFieldValid('name')}" />
                <span class="error"
                      v-if="isModelFieldValid('name')">{{getModelFieldError('name')}}</span>
            </div>
        </div>

        <ccm-loader v-if="isLoading" />
    </div>
</template>

<script>
    import CcmLoader from './ccm-loader.vue';

    export default {
        components: {
            CcmLoader
        },
        props: {
            parentId: {
                type: Number,
                default: null
            }
        },
        methods: {
            isChild: function () {
                return typeof (this.parentId) === "number";
            },
            onAddNew: function () {
                this.isFormVisible = true;
            },
            onCancel: function () {
                this.isFormVisible = false;
            },
            onAdd: function () {
                this.isLoading = true;

                let addChildUrl = "/region-manager/region-list/add-child";
                let addRootUrl = "/region-manager/region-list/add-root";

                $.ajax({
                    method: "PUT",
                    url: this.isChild()
                        ? addChildUrl
                        : addRootUrl,
                    contentType: "application/json; charset=UTF-8",
                    data: JSON.stringify(this.model),
                    success: $.proxy(function (d) {
                        this.isLoading = false;

                        if (!d.success) {
                            this.modelErrors = d;
                        } else {
                            this.isFormVisible = false;

                            this.$emit("created", d.data);
                            this.model = this.init();
                            this.modelErrors = null;
                        }
                    }, this)
                });

                console.log(JSON.stringify(this.model));
            },
            isModelFieldValid: function (fieldKey) {
                return this.getModelFieldError(fieldKey) != null;
            },
            getModelFieldError: function (fieldKey) {
                if (this.modelErrors
                    && typeof (this.modelErrors) === 'object'
                    && typeof (this.modelErrors[fieldKey]) === 'string') {
                    return this.modelErrors[fieldKey];
                } else {
                    return null;
                }
            },

            init: function () {
                return {
                    parentId: this.parentId,
                    name: null
                };
            }
        },

        data: function () {
            return {
                isFormVisible: false,
                isLoading: false,
                model: this.init(),
                modelErrors: null
            };
        }
    }
</script>