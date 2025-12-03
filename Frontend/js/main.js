const app = Vue.createApp({
    data() {
        return {
            intro: 'Welcome to my Vue template',
            logninger:[],
            device: '',
        }
    },
    methods: {
        getDataFromRaspberry(){
            axios.get(baseUri)
            .then(response => {
                this.logninger = response.data.map(item => ({
                    timestamp: item.timestamp,
                    status: item.status,
                    event: item.event,
                    address: item.address,
                    coordinates: item.coordinates,
                    selected: false
                }) )
                console.log(this.logninger)
                console.log(response.status);
            })
            .catch(error => {
                console.error("Couldn't retrieve data", error);
            });

        },
    },
    computed: {
        myComputed() {
            return ''
        },
        
    }
})