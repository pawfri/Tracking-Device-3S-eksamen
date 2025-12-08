const baseUri = 'https://mmvpt.azurewebsites.net/api/TrackingDevice';

const app = Vue.createApp({
    data() {
        return {
            loggings: [],
            deviceName: "Tanyas Taske",
            map: null,
            latestWithAddress: null,
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

        async getDataFromRaspberry() {
            try {
                const response = await axios.get(baseUri);

                // Sort newest-first before mapping
                const sorted = response.data.sort(
                (a, b) => new Date(b.timestamp) - new Date(a.timestamp));

                // Initialize logs without addresses first
                this.loggings = sorted.map(item => ({
                    timestamp: this.formatTimestamp(item.timestamp),
                    latitude: item.latitude,
                    longitude: item.longitude,
                    address: item.address,
                    source: item.source,
                    selected: false
                }));

                // Throttled sequential requests: ~2.1s between calls to avoid rate limits
                for (const log of this.loggings) {
                log.address = await this.fetchAddress(log.latitude, log.longitude);
                await new Promise(r => setTimeout(r, 2100)); // ~2.1s delay
                }

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
            
            // Handles both PascalCase and camelCase to avoid errors
            this.latestWithAddress = {
            latitude: data?.latitude ?? data?.Latitude,
            longitude: data?.longitude ?? data?.Longitude,
            date: data?.date ?? data?.Date,
            address: data?.address ?? data?.Address
            };

            console.log('Latest with address:', this.latestWithAddress);
            } 
            catch (error) {
            console.error("Couldn't retrieve latest-with-address", error);
            this.errorMessage = 'Kunne ikke hente seneste lokation med adresse';
            }
        },

        initMap() {
            if (typeof L === 'undefined') {
                console.error('Leaflet (L) is not loaded. Include Leaflet JS/CSS before this script.');
                return;
            }
            
            console.log("Startinng: Initializing Map");

            this.map = L.map('map').setView([55.630853333, 12.078415], 17);
            L.tileLayer('https://{s}.tile.openstreetmap.org/{z}/{x}/{y}.png', {
                maxZoom: 19,
                attribution: '&copy; OpenStreetMap contributors'
            }).addTo(this.map);

            console.log("Finished: Map initialized");         
        },

        async PostTrackButton() {
            try {
            const response = await axios.post(baseUri + '/trackingbutton');
            console.log("Track! saved:", response.data);

            // Reload data so the new DB entry appears immediately
            await this.getDataFromRaspberry();

            } 
            catch (error) {
            console.error("Track button failed:", error);
            }
        },

    },
    computed: {
        // This method could be useful later in our implementation for sorting logic by user
        // sortedLoggings() {
        //     return this.loggings.slice().sort((a, b) => new Date(b.timestamp) - new Date(a.timestamp));
        // } 
    }
});