var itemFilterApp = angular.module("itemFilterApp", ['ui.bootstrap']);


itemFilterApp.controller("ApiItemFilterController", function ($scope, $http) {

    $scope.loading = false;
    $scope.addMode = false;

    var status = 0;


    $scope.add = function (event) {
        $scope.loading = true;

        event.preventDefault();



        var ItemName = $('#ItemName').val();
        var ItemId = $("#ItemId").val();

    

        $http.get('/api/ApiItemFilt/', {
            params: {
                ItemName: ItemName,
                ItemId: ItemId
               


            }
        }).success(function (data) {
            var id = data[0].ItemId;
            var ItemName = data[0].ItemName;

            var filterid = data[0].FilterId;


            $('#ItemId').val(id);
            $('#ItemName').val(ItemName);
           
            if (filterid != 0) {
                $scope.ItemFilterData = data;
            }
            else {
                $scope.ItemFilterData = null;
            }

            
            $scope.loading = false;

        });

    };


    $scope.toggleEdit = function () {
        this.itemFilter.editMode = !this.itemFilter.editMode;

    };

    $scope.toggleAdd = function () {
        $scope.addMode = !$scope.addMode;
    };



    //Used to save a record after edit
    $scope.save = function () {

        if (status == 1) {
            this.itemFilter.FilterName = $('.gridFilterName').val();
            this.itemFilter.FilterId = $('.gridFilterid').val();
            this.itemFilter.FilterGroupid = $('.gridFiltergid').val();
            this.itemFilter.FilterGroupName = $('.gridFilterGroupname').val();
        } else {
            
        }
        
        //$scope.loading = true;
        var frien = this.itemFilter;

        $http.post('/api/ApiItemFilt/SaveData', this.itemFilter).success(function (data) {
            if (data.FilterId != 0) {
                alert("Saved Successfully!!");
            } else {
                alert("Duplicate data will not create");
            }
            
            frien.editMode = false;

            var ItemName = $('#ItemName').val();
            var ItemId = $("#ItemId").val();



            $http.get('/api/ApiItemFilt/', {
                params: {
                    ItemName: ItemName,
                    ItemId: ItemId

                }
            }).success(function (data) {
                var id = data[0].ItemId;
                var ItemName = data[0].ItemName;

                var filterid = data[0].FilterId;


                $('#ItemId').val(id);
                $('#ItemName').val(ItemName);

                if (filterid != 0) {
                    $scope.ItemFilterData = data;
                }
                else {
                    $scope.ItemFilterData = null;
                }



            });


            $scope.loading = false;
        }).error(function (data) {
            $scope.error = "An Error has occured while Saving Specification Data! " + data;
            $scope.loading = false;

        });
        status = 0;


    };





    $scope.addrow = function () {
        $scope.loading = false;
        event.preventDefault();
        this.newChild.ItemId = $('#ItemId').val();
        this.newChild.FilterId = $('#filterId').val();
        this.newChild.FilterGroupid = $('#filterGroupid').val();

        $http.post('/api/grid/addData', this.newChild).success(function (data, status, headers, config) {


            var ItemName = $('#ItemName').val();
            var ItemId = $("#ItemId").val();



            $http.get('/api/ApiItemFilt/', {
                params: {
                    ItemName: ItemName,
                    ItemId: ItemId



                }
            }).success(function (data) {
                var id = data[0].ItemId;
                var ItemName = data[0].ItemName;

                var filterid = data[0].FilterId;


                $('#ItemId').val(id);
                $('#ItemName').val(ItemName);

                if (filterid != 0) {
                    $scope.ItemFilterData = data;
                }
                else {
                    $scope.ItemFilterData = null;
                }


                $scope.loading = false;

            });



            if (data.FilterId != 0) {
                $('#filterName').val("");
                $('#filterGroupName').val("");
                $('#filterId').val("");
                $('#filterGroupid').val("");

                $scope.ItemFilterData.push({ Id: data.Id, ItemId: data.ItemId, FilterId: data.FilterId, FilterGroupid: data.FilterGroupid, FilterName: data.FilterName, FilterGroupName: data.FilterGroupName });
            }
            else {
                $('#filterName').val("");
                $('#filterGroupName').val("");
                $('#filterId').val("");
                $('#filterGroupid').val("");
                alert("duplicate name will not create");
            }

        }).error(function () {
            $scope.error = "An Error has occured while loading posts!";
            $scope.loading = false;
        });


    };


    $scope.deletefriend = function () {
        $scope.loading = true;
        var friendid = this.itemFilter.Id;
        $http.post('/api/ApiItemFilt/DeleteData/', this.itemFilter).success(function (data) {

            $.each($scope.ItemFilterData, function (i) {
                if ($scope.ItemFilterData[i].Id === friendid) {
                    $scope.ItemFilterData.splice(i, 1);
                    return false;
                }
            });
            $scope.loading = false;
        }).error(function (data) {
            $scope.error = "An Error has occured while Saving Friend! " + data;
            $scope.loading = false;

        });
    };

    $scope.xyz = function () {

        status = 1;
        $('.gridFilterName').autocomplete({
            
            source: function (request, response) {
                var gridfiltergid = $(".gridFiltergid").val();
                var params = {
                    type: 'GET',
                    cache: false,
                    data: {
                        query: request.term,
                        query2: gridfiltergid
                    },
                    dataType: 'json',
                    success: function (data) {
                        response($.map(data, function (item) {
                            return {
                                label: item.FilterName,
                                value: item.FilterName,
                                x: item.FilterId,
                                
                            };
                        }));
                    }
                };



              
                params.url = '/api/FilterNameSearch';

               

                $.ajax(params);



            },
            //change: groupcreate,
            select: function (event, ui) {
                $('.gridFilterName').val(ui.item.label);
                $('.gridFilterid').val(ui.item.x);
               
                

                //return true;
            }



        });

       

       

    };

    $scope.FGName = function () {

        status = 1;
        $('.gridFilterGName').autocomplete({
            source: function (request, response) {

                var params = {
                    type: 'GET',
                    cache: false,
                    data: { query: request.term },
                    dataType: 'json',
                    success: function (data) {
                        response($.map(data, function (item) {
                            return {
                                label: item.FilterGroupName,
                                value: item.FilterGroupName,
                                x: item.FilterGroupid,

                            };
                        }));
                    }
                };




                params.url = '/api/FilterGNameSearch';



                $.ajax(params);



            },
            change: filternamenull,
            select: function (event, ui) {
                $('.gridFilterGName').val(ui.item.label);
                $('.gridFiltergid').val(ui.item.x);



                //return true;
            }



        });

        function filternamenull() {
            $(".gridFilterName").val("");
            $(".gridFilterid").val("");

            


           
        }



    };

});
