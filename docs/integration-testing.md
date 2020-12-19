Kind of have some integration testing in `integration.sh` and the `--use-client-id` parameter.

I ran it through with a manually created cluster, but with a few more tweaks to the script it should be possible to provision, test, and then destroy a brand new cluster.

I'm thinking of this for big testing, large changes or Kusto version increases, because even a Dev/Test cluster is too expensive to keep around :grin:

I have the cluster stopped right now, but I need to see the cost of that even.
