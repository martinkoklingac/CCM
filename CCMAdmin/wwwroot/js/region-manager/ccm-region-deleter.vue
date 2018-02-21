<template>
    <div class="ccm-region-deleter">
        <div class="action-container"
             v-if="!isLoading">
            <button v-on:click="onCancel"
                    class="ccm-btn normal cancel-btn">
                Cancel
            </button>

            <button class="ccm-btn normal delete-btn"
                    v-on:click="onDelete">
                Delete
            </button>

        </div>
        <div class="form-container"
             v-if="!isLoading">
            <div class="warning">
                <div class="ccm-input-container normal">
                    <input class="ccm-input normal"
                           type="radio" v-bind:name="'delete-children-'+id" value="false" v-model="model.deleteChildren" />
                    <label />
                    <span v-if="!isRoot">Delete <b>{{name}}</b> &amp; merge child regions into parent</span>
                    <span v-if="isRoot">Delete <b>{{name}}</b> - child regions will become new root regions</span>
                </div>
                <div class="ccm-input-container normal">
                    <input class="ccm-input normal"
                           type="radio" v-bind:name="'delete-children-'+id" value="true" v-model="model.deleteChildren" />
                    <label />
                    <span>Delete <b>{{name}}</b> as well as entire region subtree</span>
                </div>
            </div>
        </div>

        <ccm-loader v-if="isLoading" />
    </div>
</template>

<script>
    export default {
        props: ['id', 'name', 'is-root'],
        methods: {
            onCancel: function () {
                this.$emit("cancel");
            },
            onDelete: function () {
                this.isLoading = true;

                $.ajax({
                    method: "DELETE",
                    url: "/region-manager/region-list/delete",
                    contentType: "application/json; charset=UTF-8",
                    data: JSON.stringify(this.model),
                    success: $.proxy(function (d) {
                        this.isLoading = false;

                        if (!d.success) {
                            //TODO : what will happen when this fails?
                        } else {
                            this.$emit("deleted", { deletedId: this.model.id, promotedRegions: d.data });
                            this.model = this.init();
                        }
                    }, this)
                });
            },
            init: function () {
                return {
                    id: this.id,
                    deleteChildren: false
                };
            }
        },

        data: function () {
            return {
                isLoading: false,
                model: this.init()
            }
        }
    }
</script>
