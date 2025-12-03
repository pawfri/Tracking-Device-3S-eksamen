const app = Vue.createApp({
    data() {
        return {
            intro: 'Welcome to my Vue template',
            loggings:[],
            device: '',
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
            return this.loggings.slice().sort((a, b) => {
                return new Date(b.timestamp) - new Date(a.timestamp);
            });
        },
        myComputed() {
            return ''
        },
        
    }
})