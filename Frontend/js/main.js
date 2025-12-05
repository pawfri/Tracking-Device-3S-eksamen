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
    async fetchAddress(lat, lon) {
        try {
            const response = await axios.get('https://nominatim.openstreetmap.org/reverse', {
                params: {
                    format: 'json',
                    lat: lat,
                    lon: lon
                }
            });

            const addr = response.data.address;
            if (!addr) return response.data.display_name || 'Unknown address';

            // Format som ønsket: "Maglegårdsvej 2, 4000 Roskilde, Denmark"
            return `${addr.road ?? ''} ${addr.house_number ?? ''}, ${addr.postcode ?? ''} ${addr.city ?? ''}, ${addr.country ?? ''}`;

        } catch (error) {
            console.error('Failed to fetch address', error);
            return 'Unknown address';
        }
    },
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
        async getDataFromRaspberry() {
            try {
                const response = await axios.get(baseUri);

                // Initialize logs without addresses first
                this.loggings = response.data.map(item => ({
                    timestamp: this.formatTimestamp(item.timestamp),
                    latitude: item.latitude,
                    longitude: item.longitude,
                    address: 'Henter adresse...',  // placeholder
                    selected: false
                }));

                // Fetch addresses in parallel
                await Promise.all(this.loggings.map(async log => {
                    log.address = await this.fetchAddress(log.latitude, log.longitude);
                }));

                console.log(this.loggings);

            } catch (error) {
                console.error("Couldn't retrieve data", error);
                this.errorMessage = 'Kunne ikke hente blabla';
            }
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

