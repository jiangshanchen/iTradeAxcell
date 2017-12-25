﻿@model $rootnamespace$.Models.ApplicationPermission

@{
    ViewBag.Title = "Delete";
}

<h2>Delete.</h2>

<h3>Are you sure you want to delete this Permission? </h3>
<div>
    <h4>Permission.</h4>
    <hr />
    <dl class="dl-horizontal">
        <dt>
            @Html.DisplayNameFor(model => model.Name)
        </dt>

        <dd>
            @Html.DisplayFor(model => model.Name)
        </dd>
    </dl>
    @using (Html.BeginForm())
    {
        @Html.AntiForgeryToken()

        <div class="form-actions no-color">
            <input type="submit" value="Delete" class="btn btn-default" /> |
            @Html.ActionLink("Back to List", "Index")
        </div>
    }
</div>
