const baseUri = 'https://mmvpt.azurewebsites.net/api/TrackingDevice';
const authBaseUri = 'https://mmvpt.azurewebsites.net/api/Auth';

const app = Vue.createApp({
    data() {
        return {
            currentUser: null,
            loggings: [],
            deviceName: "Tanyas Taske",
            map: null,
            latestWithAddress: null,
            currentPage: 1,
            pageSize: 10,
        }
    },

mounted() {
    const form = document.getElementById('loginForm');

    if (form) {
        // On login page: attach login form submit
        form.addEventListener('submit', this.handleLoginFormSubmit);
    } else {
        // On overview page: check login first
        this.fetchCurrentUser(true).then(() => {
            if (this.currentUser) {
                // Initialize map immediately
                this.initMap();

                // Fetch database data and latest-with-address in parallel
                Promise.all([
                    this.getDataFromDatabase(),
                    this.getLatestWithAddress()
                ])
                .then(() => {
                    console.log("All initial data loaded.");
                })
                .catch(err => {
                    console.error("Error loading initial data:", err);
                });
            }
        });
    }
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
        },

        // Login Method
        async loginUser(email, password) {
            try {
                const response = await axios.post(`${authBaseUri}/login`, {
                    email,
                    password
                }, { withCredentials: true }); // send cookies

                console.log("Login succesfuld:", response.data);

                // Only set currentUser **after server accepted login**
                this.currentUser = email;

                return true;
            } catch (error) {
                console.error("Login fejlede:", error.response?.data || error.message);
                return false;
            }
        },

        // Logout Method
        async logoutUser() {
            try {
                const response = await axios.post(`${authBaseUri}/logout`, {}, {
                    withCredentials: true
                });
                console.log("Logud succesfuld:", response.data);

                // Clear state
                this.currentUser = null;

                // Redirect to index.html
                window.location.href = 'index.html';

            } catch (error) {
                console.error("Logud fejlede:", error.response?.data || error.message);
            }
        },

        // Current User
        async fetchCurrentUser(redirectIfNone = false) {
            try {
                const response = await axios.get(`${authBaseUri}/current`, { withCredentials: true });
                this.currentUser = response.data;
                console.log("Current user:", this.currentUser);

                if (!this.currentUser && redirectIfNone && !window.location.href.endsWith('index.html')) {
                    // Only redirect if not already on index.html
                    window.location.replace('index.html');
                }
            } catch (error) {
                this.currentUser = null;
                if (redirectIfNone && !window.location.href.endsWith('index.html')) {
                    window.location.replace('index.html');
                }
                console.error("Kunne ikke hente current user", error.response?.data || error.message);
            }
        },

        // Form submit handler
        async handleLoginFormSubmit(event) {
            event.preventDefault(); // Stop normal form-submit
            const email = document.getElementById('username').value;
            const password = document.getElementById('password').value;

            // Try login
            const loginSuccess = await this.loginUser(email, password);
            if (!loginSuccess) {
                alert('Ugyldigt login!');
                return;
            }

            // Retry fetching current user until session is active
            const maxAttempts = 10;
            let currentUser = null;

            for (let attempt = 1; attempt <= maxAttempts; attempt++) {
                try {
                    const response = await axios.get(`${authBaseUri}/current`, { withCredentials: true });
                    currentUser = response.data;
                    if (currentUser) break; // Session active
                } catch (err) {
                    // Wait a bit before retrying
                    await new Promise(r => setTimeout(r, 500));
                }
            }

            if (currentUser) {
                this.currentUser = currentUser;
                // Redirect to overview page only after session is confirmed
                window.location.href = 'overview.html';
            } else {
                alert('Login session kunne ikke etableres. Prøv igen.');
            }
        },

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