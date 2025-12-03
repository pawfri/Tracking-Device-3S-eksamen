const app = Vue.createApp({
    data() {
        return {
            loggings: [],
            deviceName: '',
        }
    },
    methods: {
        getDataFromRaspberry(){
            axios.get(baseUri)
            .then(response => {
                this.loggings = response.data.map(item => ({
                    timestamp: item.timestamp,
                    status: item.status,
                    event: item.event,
                    address: item.address,
                    coordinates: item.coordinates,
                    selected: false
                }) )
                console.log(this.loggings)
                console.log(response.status);
            })
            .catch(error => {
                console.error("Couldn't retrieve data", error);
            });
        },
    },
    computed: {
        sortedLoggings() {
            return this.loggings.slice().sort((a, b) => new Date(b.timestamp) - new Date(a.timestamp));
        } 
    }
});