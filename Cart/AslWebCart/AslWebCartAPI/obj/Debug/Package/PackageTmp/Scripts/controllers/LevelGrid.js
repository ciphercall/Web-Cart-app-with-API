
var userApp = angular.module("userApp", ['ui.bootstrap']);



userApp.controller("ApiLevelEntryController", function ($scope, $http) {
   
    $scope.loading = true;
    $scope.addMode = false;
    var flag = 0;

    $scope.x = function (value) {

        flag = 1;
        var leveltype = this.grid.LevelType;

        $('.gridLevelName').val(value);
        $('.gridLevelName').autocomplete({
            source: function (request, response) {

                var params = {
                    type: 'GET',
                    cache: false,
                    data: { query: request.term },
                    dataType: 'json',
                    success: function (data) {
                        response($.map(data, function (item) {
                            return {
                                label: item.Text,
                                value: item.Text,
                                x: item.Value
                            };
                        }));
                    }
                };



                if (leveltype == "CATEGORY") {
                    params.url = '/api/test';

                } else if (leveltype == "ITEM-GROUP") {
                    params.url = '/api/ItemWiseLevel';

                } else if (leveltype == "ITEM-SINGLE") {
                    params.url = '/api/ItemsingleWiseLevel';

                }

                $.ajax(params);



            },
            select: function (event, ui) {
                $('.gridLevelName').val(ui.item.label);
                $('.gridLevelId').val(ui.item.x);

               
           
                return true;
            },
            minLength: 1,



        });




    };

  

    $scope.add = function (event) {
        $scope.loading = true;

        event.preventDefault();

        LevelGroupType = $('#LevelGroupType option:selected').val();
        LevelGroupId = $('#LevelGroupId').val();
        LevelGroupName = $('#LevelGroupName').val();

        $http.get('/api/grid/GetData/', {
            params: {
                LevelGroupType: LevelGroupType,
                LevelGroupId: LevelGroupId,
                LevelGroupName: LevelGroupName

            }
        }).success(function (data) {
            var levelid = data[0].LevelId;
            if (levelid == 0) {

                $scope.gridValue = null;
                
            } else {
                $scope.gridValue = data;
            }
            

        });
    };



    ///////////////////// Row add  ////////////////////////////

    $scope.addrow = function () {

        $scope.loading = false;

        event.preventDefault();
        //alert("Success");

        this.newChild.LevelGroupType = $('#LevelGroupType').val();
        this.newChild.LevelGroupId = $('#LevelGroupId').val();
        this.newChild.LevelGroupName = $('#LevelGroupName').val();
        this.newChild.LevelName = $('#LevelName').val();
        this.newChild.LevelId = $('#LevelId').val();
        this.newChild.LevelNameNew = $('#LevelNameNew').val();
    
            $http.post('/api/grid/GridRowData', this.newChild).success(function(data, status, headers, config) {

                    $('#LevelType').val('');
                    $('#LevelName').val("");
                    $('#LevelId').val("");
                    $('#LevelNameNew').val("");

                    if (data.LevelId!=0) {
                        $scope.gridValue.push({ LevelGroupId: data.LevelGroupId, Id: data.Id, LevelType: data.LevelType, LevelId: data.LevelId, LevelName: data.LevelName, LevelNameNew: data.LevelNameNew });
                    } else {
                        alert("duplicate data");
                    }
                    
                    //$scope.gridValue = data;
                })
                .error(function() {
                    $scope.error = "An Error has occured while loading posts!";
                    $scope.loading = false;
                });
         
       


    };

    ///////////////////// edit  ////////////////////////////

    $scope.toggleEdit = function () {
       
       
      
        this.grid.editMode = !this.grid.editMode;
       

    };

    //Used to save a record after edit
    $scope.save = function () {
        // alert("Edit");
        $scope.loading = true;
        if (flag == 1) {

            this.grid.LevelId = $('#idgridLevelID').val();

            this.grid.LevelName = $('#idgridLevelName').val();
            this.gridValue.LevelNameNew = $('#gridLevelNameNew').val();
        } else {
            
        }
      
       
        var frien = this.grid;
       
        $http.post('/api/ApiLevelEntry/SaveData', this.grid).success(function (data, status, headers, config) {
            if (data.LevelId != 0) {
                alert("Saved Successfully!!");
            } else {
                alert("Duplicate value entered");
            }
           
            frien.editMode = false;

            LevelGroupType = $('#LevelGroupType option:selected').val();
            LevelGroupId = $('#LevelGroupId').val();
            LevelGroupName = $('#LevelGroupName').val();

            $http.get('/api/grid/GetData/', {
                params: {
                    LevelGroupType: LevelGroupType,
                    LevelGroupId: LevelGroupId,
                    LevelGroupName: LevelGroupName

                }
            }).success(function (data) {
                var levelid = data[0].LevelId;
                if (levelid == 0) {

                    $scope.gridValue = null;

                } else {
                    $scope.gridValue = data;
                }


            });
            $('.gridLevelName').val("");
            $('.gridLevelId').val("");
           
            $scope.loading = false;
        }).error(function (data) {
            $scope.error = "An Error has occured while Saving Friend! " + data;
            $scope.loading = false;

        });

        flag = 0;

    };



    ///////////////////// delete  ////////////////////////////


    $scope.deletegrid = function () {
        $scope.loading = true;
        var gridid = this.grid.Id;
     
        $http.post('/api/ApiLevelEntry/DeleteData', this.grid).success(function (data,status, headers, config) {
            alert("Deleted Successfully!!");
            $.each($scope.gridValue, function (i) {
                if ($scope.gridValue[i].Id === gridid) {
                    $scope.gridValue.splice(i, 1);
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