

Vue.component("ccm-region-editor", {
    props: ['id'],
    template:
    `<div>
        <div>Region Editor</div>
        <div>For : [{{id}}]</div>
        <div>
            <button v-on:click="onCancel">Cancel</button>
        </div>
    </div>`,

    methods: {
        onCancel: function () {
            this.$emit("cancel");
        }
    }
});

Vue.component("ccm-region", {
    props: ['id', 'name'],
    template:
    `<div>
        <div>
            <span>{{name}}</span>
            <button v-on:click="onEdit">Edit</button>
        </div>
        <div>
            <ccm-region-editor 
                v-bind:id='id' 
                v-if="mode === 'Edit'" 
                v-on:cancel="onCancelled">
            </ccm-region-editor>
        </div>
    </div>`,
    methods: {
        onEdit: function () {
            this.mode = "Edit";
        },
        onCancelled: function () {
            this.mode = "None";
        }
    },

    data: function () {
        return {
            mode: 'None'
        };
    }
})

let vm = new Vue({
    el: '#_regionList'
});