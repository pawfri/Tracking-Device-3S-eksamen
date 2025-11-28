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
                this.logninger = response.data
                console.log(this.logninger)
                console.log(response.status);
            })
            .catch(error => {
                console.error("Kunne ikke hente data og sætte dem ind i listen", error);
            });

        },
    },
    computed: {
        myComputed() {
            return ''
        },
        
    }
})