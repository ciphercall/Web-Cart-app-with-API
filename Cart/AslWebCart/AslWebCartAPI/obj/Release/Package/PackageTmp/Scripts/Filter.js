var filterApp = angular.module("filterApp", ['ui.bootstrap']);


filterApp.controller("ApiFilterItemController", function ($scope, $http) {

    $scope.loading = false;
    $scope.addMode = false;

    

   

    $scope.add = function (event) {
        $scope.loading = true;

        event.preventDefault();



        var name = $('#GroupName').val();
        var groupid = $("#txtFilgroupid").val();

        $http.get('/api/ApiFilterItem/GetData/', {
            params: {
                GroupName: name,
                Groupid: groupid


            }
        }).success(function (data) {
            var id = data[0].FilterGroupid;
            var filterid = data[0].FilterId;
            $('#txtFilgroupid').val(id);
            if (id != 0 && filterid != 0){
                $scope.filterData = data;
            }
            else
            {
                $scope.filterData = null;
            }

            $scope.loading = false;

        });

    };
  

    $scope.toggleEdit = function () {
        this.filteritem.editMode = !this.filteritem.editMode;

    };

    $scope.toggleAdd = function () {
        $scope.addMode = !$scope.addMode;
    };


    //Used to save a record after edit
    $scope.save = function () {
        // alert("Edit");
        $scope.loading = true;
        var frien = this.filteritem;
       
        $http.post('/api/ApiFilterItem/SaveData', this.filteritem).success(function (data) {
            if (data.FilterId != 0) {
                alert("Saved Successfully!!");
            } else {
                alert("Duplicate data entered");
            }
           
            frien.editMode = false;
           
            var name = $('#GroupName').val();
            var groupid = $("#txtFilgroupid").val();

            $http.get('/api/ApiFilterItem/GetData/', {
                params: {
                    GroupName: name,
                    Groupid: groupid


                }
            }).success(function (data) {
                var id = data[0].FilterGroupid;
                var filterid = data[0].FilterId;
                $('#txtFilgroupid').val(id);
                if (id != 0 && filterid != 0) {
                    $scope.filterData = data;
                }
                else {
                    $scope.filterData = null;
                }

              

            });

            $scope.loading = false;
        }).error(function (data) {
            $scope.error = "An Error has occured while Saving Friend! " + data;
            $scope.loading = false;

        });
    };





    $scope.addrow = function () {
        $scope.loading = false;
        event.preventDefault();
        this.newChild.FilterGroupid = $('#txtFilgroupid').val();
        if (this.newChild.FilterGroupid != "") {
            $http.post('/api/grid/FilterChild', this.newChild).success(function(data, status, headers, config) {
                var name = $('#GroupName').val();
                var groupid = $("#txtFilgroupid").val();

                $http.get('/api/ApiFilterItem/', {
                    params: {
                        GroupName: name,
                        Groupid: groupid


                    }
                }).success(function(data) {
                    var id = data[0].FilterGroupid;
                    var filterid = data[0].FilterId;
                    $('#txtFilgroupid').val(id);
                    if (id != 0 && filterid != 0) {
                        $scope.filterData = data;
                    } else {
                        $scope.filterData = null;
                    }

                    $scope.loading = false;

                });


                if (data.FilterId != 0) {
                    $('#filternm').val("");
                    $('#filternid').val("");
                    $('#filtersid').val("");
                    $scope.filterData.push({ Id: data.Id, FilterGroupid: data.FilterGroupid, FilterId: data.FilterId, FilterName: data.FilterName, FilterNumericId: data.FilterNumericId, FilterShortId: data.FilterShortId });
                } else {
                    $('#filternm').val("");
                    $('#filternid').val("");
                    $('#filtersid').val("");
                    alert("duplicate name will not create");
                }

            }).error(function() {
                $scope.error = "An Error has occured while loading posts!";
                $scope.loading = false;
            });

        } else {
            $('#filternm').val("");
            $('#filternid').val("");
            $('#filtersid').val("");
            alert("Enter Filter HeadName First");
        }

    };


    $scope.deletefriend = function () {
        $scope.loading = true;
        var friendid = this.filteritem.Id;
        $http.post('/api/ApiFilterItem/DeleteData/', this.filteritem).success(function (data) {
          
            $.each($scope.filterData, function (i) {
                if ($scope.filterData[i].Id === friendid) {
                    $scope.filterData.splice(i, 1);
                    return false;
                }
            });
            $scope.loading = false;
        }).error(function (data) {
            $scope.error = "An Error has occured while Saving Friend! " + data;
            $scope.loading = false;

        });
    };





});
