﻿using CityInfo.API.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CityInfo.API.Controllers;

[Route("api/cities/{cityId}/pointsofinterest")]
[ApiController]
//[Authorize]
public class PointsOfInterestController : ControllerBase
{
    [HttpGet]
    public ActionResult<IEnumerable<PointOfInterestDto>> GetPointsOfInterest(int cityId)
    {
        var city = CitiesDataStore.Current.Cities.FirstOrDefault(c => c.Id == cityId);

        if (city == null)
        {
            return NotFound("City does not exist for the Id: " + cityId);
        }

        return Ok(city.PointsOfInterest);
    }

    [HttpGet("{pointOfInterestId}", Name = "GetPointOfInterest")]
    public ActionResult<PointOfInterestDto> GetPointOfInterest(int cityId, int pointOfInterestId)
    {
        var city = CitiesDataStore.Current.Cities.FirstOrDefault(c => c.Id == cityId);
        if (city == null)
        {
            return NotFound("City does not exist for cityId: " + cityId);
        }

        var pointOfInterest = city.PointsOfInterest.FirstOrDefault(p => p.Id == pointOfInterestId);
        if (pointOfInterest == null)
        {
            return NotFound("Point of interest does not exist for the Id: " + pointOfInterestId);
        }

        return Ok(pointOfInterest);
    }

    [HttpPost]
    public ActionResult<PointOfInterestDto> CreatePointOfInterest(
        int cityId,
        PointOfInterestForCreationDto pointOfInterest
    )
    {
        var city = CitiesDataStore.Current.Cities.FirstOrDefault(c => c.Id == cityId);
        if (city is null)
        {
            return NotFound();
        }

        var maxPointOfInterestId = CitiesDataStore
            .Current.Cities.SelectMany(c => c.PointsOfInterest)
            .Max(p => p.Id);

        var finalPointOfInterest = new PointOfInterestDto()
        {
            Id = ++maxPointOfInterestId,
            Name = pointOfInterest.Name,
            Description = pointOfInterest.Description
        };

        city.PointsOfInterest.Add(finalPointOfInterest);

        return CreatedAtRoute(
            "GetPointOfInterest",
            new { cityId = cityId, pointOfInterestId = finalPointOfInterest.Id },
            finalPointOfInterest
        );
    }

    [HttpPut("{pointofinterestid}")]
    public ActionResult UpdatePointOfInterest(
        int cityId,
        int pointOfInterestId,
        PointOfInterestForUpdateDto pointOfInterest
    )
    {
        var city = CitiesDataStore.Current.Cities.FirstOrDefault(c => c.Id == cityId);
        if (city is null)
        {
            return NotFound();
        }

        // find point of interest
        var pointOfInterestFromStore = city.PointsOfInterest.FirstOrDefault(p =>
            p.Id == pointOfInterestId
        );

        if (pointOfInterestFromStore is null)
        {
            return NotFound();
        }

        pointOfInterestFromStore.Name = pointOfInterest.Name;
        pointOfInterestFromStore.Description = pointOfInterest.Description;

        return NoContent();
    }
}
