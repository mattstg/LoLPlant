using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cloud : MonoBehaviour {

    
    protected Vector2 speedRange;
    protected Vector2 altitudeRange;
    protected Vector2 travelRange;
    protected float sortingLayerCriticalValue;

    protected SpriteRenderer sprite;
    protected float speed;
    protected float altitude;


    public virtual void Initialize(bool singleLockedScreen)
    {
        sprite = this.GetComponent<SpriteRenderer>();

        speedRange = GV.cloudSpeedRange;

        if (!singleLockedScreen)
        {
            altitudeRange = GV.cloudAltitudeRange;
            travelRange = GV.cloudTravelRange;
        }
        else
        {
            altitudeRange = GV.cloudSingleScreenAltitudeRange;
            travelRange = GV.cloudSingleScreenTravelRange;
        }
        sortingLayerCriticalValue = GV.sortingLayerCriticalValue;

        speed = Random.Range(speedRange.x, speedRange.y);
        altitude = Random.Range(altitudeRange.x, altitudeRange.y);

        if (speed <= sortingLayerCriticalValue)
            sprite.sortingLayerName = "clouds behind mtns";
        else if (speed > sortingLayerCriticalValue)
            sprite.sortingLayerName = "clouds in front of mtns";

        this.transform.Translate(0, altitude - this.transform.localPosition.y, 0);
    }

    public virtual void Refresh(float dt)
    {
        if (this.transform.localPosition.x >= travelRange.x) //if still in bounds, keep moving left
            this.transform.Translate(-speed * dt, 0, 0);
      
        else if (this.transform.localPosition.x < travelRange.x) //if past left limit, respawn on right
            Reinitialize();
    }

    public virtual void Reinitialize() //called when a cloud passes far enough past the left edge of the background to 'respawn' on the right
    {
        this.transform.Translate(2 * travelRange.y, 0, 0);
        speed = Random.Range(speedRange.x, speedRange.y);
        altitude = Random.Range(altitudeRange.x, altitudeRange.y);

        if (speed <= sortingLayerCriticalValue)
            sprite.sortingLayerName = "clouds behind mtns";
        else if (speed > sortingLayerCriticalValue)
            sprite.sortingLayerName = "clouds in front of mtns";

        this.transform.Translate(0, altitude - this.transform.localPosition.y, 0);
    }
}
