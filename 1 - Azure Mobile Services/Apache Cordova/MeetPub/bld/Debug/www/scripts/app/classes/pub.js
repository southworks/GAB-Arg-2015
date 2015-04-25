var Pub = (function () {
    function Pub(options) {
        this.Id = options.id || '';
        this.Name = options.name || '';
        this.Description = options.description || '',
        this.Direction = options.direction || '';
        this.Going = options.going || 0;
        this.GeoLocation = {
            Latitud: options.latitude || 0,
            Longitud: options.longitude || 0
        };
        this.Reviews = options.reviews || [];
    }

    Pub.prototype.Url = function () {
        return "/pub/" + this.Id;
    };

    return Pub;
}());