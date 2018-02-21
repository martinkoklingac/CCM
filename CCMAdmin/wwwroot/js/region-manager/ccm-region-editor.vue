<template>
    <div class="ccm-region-editor">
        <div>Region Editor</div>
        <div>For : [{{id}}]</div>
        <div>
            <button v-on:click="onCancel" class="ccm-btn normal cancel-btn">Cancel</button>
        </div>
    </div>
</template>

<script>
    export default {
        props: ['id'],
        methods: {
            onCancel: function () {
                this.$emit("cancel");
            }
        }
    }
</script>