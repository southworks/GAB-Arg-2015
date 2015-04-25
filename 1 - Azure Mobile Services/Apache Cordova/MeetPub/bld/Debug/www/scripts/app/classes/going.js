var Going = (function () {
    function Going(option) {
        this.Pub = option.Id || 0;
        this.Date = option.Date || new Date();
    }

    return Going;
}())