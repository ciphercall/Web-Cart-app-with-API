var specApp = angular.module("specApp", ['ui.bootstrap']);


specApp.controller("ApiSpecificationController", function ($scope, $http) {

    $scope.loading = false;
    $scope.addMode = false;




    $scope.add = function (event) {
        $scope.loading = true;

        event.preventDefault();



        var ItemName = $('#ItemName').val();
        var ItemId = $("#ItemId").val();

        var SpecificationSL = $('#SpecificationSL').val();
        var SpecificationName = $("#SpecificationName").val();

        var ViewSL = $('#ViewSL').val();


        $http.get('/api/ApiSpecification/', {
            params: {
                ItemName: ItemName,
                ItemId: ItemId,
                SpecificationSL: SpecificationSL,
                SpecificationName: SpecificationName,
                ViewSL: ViewSL


            }
        }).success(function (data) {
            var id = data[0].ItemId;
            var specificationsl = data[0].SpecificationSL;
          
            var featuresl = data[0].FeatureSL;


            $('#ItemId').val(id);
            $('#ItemName').val(ItemName);
            $('#SpecificationSL').val(specificationsl);
            $('#ViewSL').val(ViewSL);

            if (id != 0 && featuresl != 0) {
                $scope.specificationData = data;
            }
            else {
                $scope.specificationData = null;
            }
           
            $scope.loading = false;

        });

    };
   

    $scope.toggleEdit = function () {
        this.specItem.editMode = !this.specItem.editMode;

    };

  


    //Used to save a record after edit
    $scope.save = function () {
      
        $scope.loading = true;
        var frien = this.specItem;
   
        $http.post('/api/ApiSpecification/SaveData', this.specItem).success(function (data) {
            if (data.FeatureSL != 0) {
                alert("Saved Successfully!!");
            } else {
                alert("duplicate data entered");
            }
           
            frien.editMode = false;
          
            var ItemName = $('#ItemName').val();
            var ItemId = $("#ItemId").val();

            var SpecificationSL = $('#SpecificationSL').val();
            var SpecificationName = $("#SpecificationName").val();

            var ViewSL = $('#ViewSL').val();


            $http.get('/api/ApiSpecification/', {
                params: {
                    ItemName: ItemName,
                    ItemId: ItemId,
                    SpecificationSL: SpecificationSL,
                    SpecificationName: SpecificationName,
                    ViewSL: ViewSL


                }
            }).success(function (data) {
                var id = data[0].ItemId;
                var specificationsl = data[0].SpecificationSL;

                var featuresl = data[0].FeatureSL;


                $('#ItemId').val(id);
                $('#ItemName').val(ItemName);
                $('#SpecificationSL').val(specificationsl);
                $('#ViewSL').val(ViewSL);

                if (id != 0 && featuresl != 0) {
                    $scope.specificationData = data;
                }
                else {
                    $scope.specificationData = null;
                }

               

            });


            $scope.loading = false;
        }).error(function (data) {
            $scope.error = "An Error has occured while Saving Specification Data! " + data;
            $scope.loading = false;

        });
    };





    $scope.addrow = function () {
        $scope.loading = false;
        event.preventDefault();
        this.newChild.ItemId = $('#ItemId').val();
        this.newChild.SpecificationSL = $('#SpecificationSL').val();

        $http.post('/api/grid/SpecChild', this.newChild).success(function (data, status, headers, config) {

            var ItemName = $('#ItemName').val();
            var ItemId = $("#ItemId").val();

            var SpecificationSL = $('#SpecificationSL').val();
            var SpecificationName = $("#SpecificationName").val();

            var ViewSL = $('#ViewSL').val();


            $http.get('/api/ApiSpecification/', {
                params: {
                    ItemName: ItemName,
                    ItemId: ItemId,
                    SpecificationSL: SpecificationSL,
                    SpecificationName: SpecificationName,
                    ViewSL: ViewSL


                }
            }).success(function (data) {
                var id = data[0].ItemId;
                var specificationsl = data[0].SpecificationSL;

                var featuresl = data[0].FeatureSL;


                $('#ItemId').val(id);
                $('#ItemName').val(ItemName);
                $('#SpecificationSL').val(specificationsl);
                $('#ViewSL').val(ViewSL);

                if (id != 0 && featuresl != 0) {
                    $scope.specificationData = data;
                }
                else {
                    $scope.specificationData = null;
                }

                $scope.loading = false;

            });




            if (data.FeatureSL != 0) {
                $('#featureType').val("");
                $('#feature').val("");
                $scope.specificationData.push({ Id: data.Id, ItemId: data.ItemId, SpecificationSL: data.SpecificationSL, FeatureSL: data.FeatureSL, FeatureType: data.FeatureType, Feature: data.Feature });
                
            }

            else {
                $('#featureType').val("");
                $('#feature').val("");
                alert("duplicate name will not create");
            }
           
        }).error(function () {
            $scope.error = "An Error has occured while loading posts!";
            $scope.loading = false;
        });



    };


    $scope.deletefriend = function () {
        $scope.loading = true;
        var friendid = this.specItem.Id;
        $http.post('/api/ApiSpecification/DeleteData/' ,this.specItem).success(function (data) {

            $.each($scope.specificationData, function (i) {
                if ($scope.specificationData[i].Id === friendid) {
                    $scope.specificationData.splice(i, 1);
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
