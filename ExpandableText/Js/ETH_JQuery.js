function ShowOrHideContentJquery(idOfElement, itemId) 
{
    //Create paths
    expand = jQuery("#ICG_ETH_EXPAND_" + itemId);
    collapse = jQuery("#ICG_ETH_COLLAPSE_" + itemId);


    //do the magic
    listing = jQuery("#" + idOfElement);
    if (listing.hasClass('hideContent')) {
        //Currently hidden, show with jquery
        listing.show('slow').removeClass('hideContent');

        //Setup expand collapse
        if (expand != null) {
            expand.addClass('hideContent');
            collapse.removeClass('hideContent');
        }
    }
    else {
        //Show
        listing.hide('slow').addClass('hideContent');

        //Setup expand collapse
        if (expand != null) {
            expand.removeClass('hideContent');
            collapse.addClass('hideContent');
        }
    }
}

function ICG_ETH_ShowContent(idOfElement, itemId) 
{
    //Create paths
    expand = jQuery("#ICG_ETH_EXPAND_" + itemId);
    collapse = jQuery("#ICG_ETH_COLLAPSE_" + itemId);


    //do the magic
    listing = jQuery("#" + idOfElement);
    if (listing.hasClass('hideContent')) {
        //Currently hidden, show with jquery
        listing.show('slow').removeClass('hideContent');

        //Setup expand collapse
        if (expand != null) {
            expand.addClass('hideContent');
            collapse.removeClass('hideContent');
        }
    }
    else {
        //Do nothing, content was already visible!
    }
}

function ICG_ETH_HideContent(idOfElement, itemId) 
{
    //Create paths
    expand = jQuery("#ICG_ETH_EXPAND_" + itemId);
    collapse = jQuery("#ICG_ETH_COLLAPSE_" + itemId);


    //do the magic
    listing = jQuery("#" + idOfElement);
    if (listing.hasClass('hideContent')) {
        //Do Nothing, already hidden
    }
    else {
        //Show
        listing.hide('slow').addClass('hideContent');

        //Setup expand collapse
        if (expand != null) {
            expand.removeClass('hideContent');
            collapse.addClass('hideContent');
        }
    }
}

function ICG_ETH_LoadFromUrl() 
{
    if (location.hash) 
    {
        var contentId = location.hash.replace("#", "");
        var itemId = contentId.substring(contentId.lastIndexOf("_") + 1);
        ICG_ETH_ShowContent(contentId, itemId);
    }
}
