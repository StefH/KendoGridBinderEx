(function () {
    "use strict";

    var module = angular.module("mainModule");

    module.controller("GridManagersController", function ($scope, $http) {

        $scope.refresh = function () {
            // scope.grid is the widget reference
            $scope.grid.refresh();
        }

        $scope.managerGridDatasource = {
            serverPaging: true,
            serverSorting: true,
            serverFiltering: true,
            pageSize: 5,
            transport: {
                read: {
                    type: 'post',
                    dataType: 'json',
                    url: '@Url.Action("GridManagers")'
                },
            },
            schema: {
                data: 'Data',
                total: 'Total',
                model: {
                    id: 'Id',
                    fields: {
                        Number: { type: 'number' },
                        First: { type: 'string' },
                        Last: { type: 'string' },
                        Full: { type: 'string' },
                        Email: { type: 'string' }
                    }
                }
            }
        };

        $scope.managerGridOptions = {
            sortable: true,
            pageable: true,
            columns: [
                { field: 'Number', title: 'Employee #' },
                { field: 'First', title: 'FirstName' },
                { field: 'LastName', title: 'Last' },
                { field: 'Full', title: 'FullName' },
                { field: 'Email' }
            ]
        };

    });

})();