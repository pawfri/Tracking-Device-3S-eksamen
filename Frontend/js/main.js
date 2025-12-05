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
        this.getLatestWithAddress();
    },

    methods: {
        formatTimestamp(ts) {
            const d = new Date(ts);

            const day = String(d.getDate()).padStart(2, '0');
            const month = String(d.getMonth() + 1).padStart(2, '0');
            const year = d.getFullYear();

            const hours = String(d.getHours()).padStart(2, '0');
            const minutes = String(d.getMinutes()).padStart(2, '0');
            const seconds = String(d.getSeconds()).padStart(2, '0');

            return `${day}-${month}-${year} ${hours}:${minutes}:${seconds}`;
        },
        getDataFromRaspberry(){
            axios.get(baseUri)
            .then(response => {
                this.loggings = response.data.map(item => ({
                    timestamp: this.formatTimestamp(item.timestamp),
                    // status: item.status,
                    // event: item.event,
                    address: this.getLatestWithAddress(item.address),
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
                this.errorMessage = 'kunne ikke hente blabla';
            });
        },
        async getLatestWithAddress() {
            try {
            const response = await axios.get(`${baseUri}/latest-with-address`);
            const data = response?.data;
        
            // Håndter både camelCase og PascalCase (uden if ved
            // brug af ?? og optional chaining)
            this.latestWithAddress = {
            latitude: data?.latitude ?? data?.Latitude,
            longitude: data?.longitude ?? data?.Longitude,
            date: data?.date ?? data?.Date,
            address: data?.address ?? data?.Address
            };
            console.log('Latest with address:',
            this.latestWithAddress);
            } catch (error) {
            console.error("Couldn't retrieve latest-with-address",
            error);
            this.errorMessage = 'Kunne ikke hente seneste lokation med adresse';
            }
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

