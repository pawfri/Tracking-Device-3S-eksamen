const baseUri = 'https://mmvpt.azurewebsites.net/api/TrackingDevice';

const app = Vue.createApp({
    data() {
        return {
            loggings: [],
            deviceName: "Tanyas Taske",
            map: null,
            latestWithAddress: null,
            currentPage: 1,
            pageSize: 10,
        }
    },

     mounted() {
        this.initMap();
        this.getDataFromDatabase();
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

        async getDataFromDatabase() {
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
            await this.getDataFromDatabase();

            } 
            catch (error) {
            console.error("Track button failed:", error);
            }
        },

        // Methods related to pages in the table of location entries
        goToPage(page) {
            if (page >= 1 && page <= this.totalPages) {
                this.currentPage = page;
            }
        },
        nextPage() {
            this.goToPage(this.currentPage + 1);
        },
        prevPage() {
            this.goToPage(this.currentPage - 1);
        },
        firstPage() {
            this.goToPage(1);
        },
        lastPage() {
            this.goToPage(this.totalPages);
        }

    },
    computed: {
    // Slice the array when reaching the set pageSize defined as a dataobject
    pagedLoggings() {
        const start = (this.currentPage - 1) * this.pageSize;
        const end = start + this.pageSize;
        return this.loggings.slice(start, end);
    },

    // Total number of pages in the table
    totalPages() {
        return Math.ceil(this.loggings.length / this.pageSize);
    },

    // Which page numbers to show in the pagination control
    visiblePageNumbers() {
        const pages = [];
        const maxButtons = 5; // Set how many page numbers around current page
        let start = Math.max(1, this.currentPage - 2);
        let end = Math.min(this.totalPages, start + maxButtons - 1);

        // Adjust start if near the end
        start = Math.max(1, end - maxButtons + 1);

        for (let i = start; i <= end; i++) {
            pages.push(i);
        }

        return pages;
        }
    }
});