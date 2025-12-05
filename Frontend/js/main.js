const baseUri = 'https://mmvpt.azurewebsites.net/api/TrackingDevice';

const app = Vue.createApp({
    data() {
        return {
            loggings: [],
            deviceName: '',
            map: null,
        }
    },

     mounted() {
        this.initMap();
        this.getDataFromRaspberry();
    },

    methods: {
        getDataFromRaspberry(){
            axios.get(baseUri)
            .then(response => {
                this.loggings = response.data.map(item => ({
                    timestamp: item.timestamp,
                    // status: item.status,
                    // event: item.event,
                    address: item.address,
                    latitude: item.latitude,
                    longitude: item.longitude,
                    // coordinates: item.coordinates,
                    selected: false
                }) )
                console.log(this.loggings)
                console.log(response.status);
            })
            .catch(error => {
                console.error("Couldn't retrieve data", error);
            });
        },
        initMap() {
                    if (typeof L === 'undefined') {
                        console.error('Leaflet (L) is not loaded. Include Leaflet JS/CSS before this script.');
                        return;
                    }
                    console.log("Initialiserer kortet");
                    this.map = L.map('map').setView([51.505, -0.09], 18);
                    L.tileLayer('https://{s}.tile.openstreetmap.org/{z}/{x}/{y}.png', {
                        maxZoom: 19,
                        attribution: '&copy; OpenStreetMap contributors'
                    }).addTo(this.map);
                    console.log("Kort initialiseret");
                    
                }

      
    },
    computed: {
        sortedLoggings() {
            return this.loggings.slice().sort((a, b) => new Date(b.timestamp) - new Date(a.timestamp));
        } 
    }
});

