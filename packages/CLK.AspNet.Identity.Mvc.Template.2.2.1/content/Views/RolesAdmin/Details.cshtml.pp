﻿@model $rootnamespace$.Models.ApplicationRole

@{
    ViewBag.Title = "Details";
}

<h2>Details.</h2>
<br />

<h4>This role</h4>
<table class="table">
    <tr>
        <td style="width:80pt;">
            @Html.DisplayNameFor(model => model.Name)
        </td>
        <td>
            @Html.DisplayFor(model => model.Name)
        </td>
    </tr>
</table><br />


<h4>List of users in this role</h4>
@if (ViewBag.UserCount == 0)
{
    <hr />
    <p>No users found in this role.</p>
}

<table class="table">

    @foreach (var item in ViewBag.Users)
    {
        <tr>
            <td>
                @item.UserName
            </td>
        </tr>
    }
</table><br />


<h4>List of permissions in this role</h4>
@if (ViewBag.PermissionCount == 0)
{
    <hr />
    <p>No permissions found in this role.</p>
}

<table class="table">

    @foreach (var item in ViewBag.Permissions)
    {
        <tr>
            <td>
                @item.Name
            </td>
        </tr>
    }
</table><br />


<p>
    @Html.ActionLink("Edit", "Edit", new { id = Model.Id }) |
    @Html.ActionLink("Back to List", "Index")
</p>



