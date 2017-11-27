﻿
Vue.component("ccm-loader", {
    template:
    `<div class="loader">
        <span class="loader-item"></span>
        <span class="loader-item"></span>
        <span class="loader-item"></span>
        <span class="loader-item"></span>
    </div>`
});


Vue.component("ccm-region-editor", {
    props: ['id'],
    template:
    `<div class="ccm-region-editor">
        <div>Region Editor</div>
        <div>For : [{{id}}]</div>
        <div>
            <button v-on:click="onCancel" class="ccm-btn normal cancel-btn">Cancel</button>
        </div>
    </div>`,

    methods: {
        onCancel: function () {
            this.$emit("cancel");
        }
    }
});

Vue.component("ccm-region-constructor", {
    props: ['parent-id'],
    template:
    `<div class="ccm-region-constructor">
        <div class="action-container"
            v-if="!isLoading">
            <button
                v-if="!isFormVisible"
                v-on:click="onAddNew"
                class="ccm-btn normal submit-btn">Add New</button>
            <button
                v-if="isFormVisible"
                v-on:click="onCancel"
                class="ccm-btn normal cancel-btn">Cancel</button>
            <button
                v-if="isFormVisible"
                v-on:click="onAdd"
                class="ccm-btn normal submit-btn">Add</button>
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
    </div>`,

    methods: {
        onAddNew: function () {
            this.isFormVisible = true;
        },
        onCancel: function () {
            this.isFormVisible = false;
        },
        onAdd: function () {
            this.isLoading = true;

            $.ajax({
                method: "PUT",
                url: "/region-manager/region-list/add",
                contentType: "application/json; charset=UTF-8",
                data: JSON.stringify(this.model),
                success: $.proxy(function (d) {
                    this.isLoading = false;
            
                    if (!d.success) {
                        this.modelErrors = d;
                    } else {
                        console.log("-> OK!");
                        console.log(d);
                        this.isFormVisible = false;

                        this.$emit("created", { id: 123, name: this.model.name });
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
});

Vue.component("ccm-region", {
    props: ['id', 'parent-id', 'name'],
    template:
    `<div class="ccm-region" v-bind:class="{ 'child': isChild, 'root': !isChild, 'expanded': isExpanded}">
        <div class="region-wrapper">
            <div class="info-section">
                <span>{{name}}</span>
            </div>
            <div class="action-section">
                <button
                    v-on:click="onExpand"
                    v-if="isExpanded === false"
                    class="ccm-btn normal submit-btn">Expand</button>
                <button
                    v-on:click="onCollapse"
                    v-if="isExpanded"
                    class="ccm-btn normal submit-btn">Collapse</button>

                <button 
                    v-on:click="onEdit"
                    :disabled="mode === 'Edit'"
                    class="ccm-btn normal submit-btn">Edit</button>
            </div>
        </div>
        

        <div>
            <ccm-region-editor 
                v-bind:id='id' 
                v-if="mode === 'Edit'" 
                v-on:cancel="onCancelled">
            </ccm-region-editor>
        </div>

        <div v-if="isExpanded"
            class="region-constructor-wrapper">
            <ccm-region-constructor
                v-if="isExpanded"
                v-bind:parent-id = 'id'
                v-on:created="onRegionCreated"/>
        </div>

        <div v-if="isExpanded" 
            class="child-region-wrapper">
            <ccm-region
                v-for="region in childRegions" 
                v-bind:id='region.id'
                v-bind:parent-id = 'id'
                v-bind:name='region.name' />
            <div
                v-if="childRegions.length == 0">
                There are no regions
            </div>
        </div>
    </div>`,
    methods: {
        onEdit: function () {
            this.mode = "Edit";
        },
        onCancelled: function () {
            this.mode = "None";
        },
        onExpand: function () {
            if (!this.childRegions) {
                this.childRegions = [
                    { id: 77, name: "dummy name 77" },
                    { id: 88, name: "dummy name 88" }
                ]
            }

            this.isExpanded = true;
        },
        onCollapse: function () {
            this.isExpanded = false;
        },
        onRegionCreated: function (d) {
            console.log("-> created");
            console.log(d);
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
            isChild: false,
            childRegions: null
        };
    }
})

let vm = new Vue({
    el: '#_regionList'
});