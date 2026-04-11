{
  description = "Development shell";

  inputs.nixpkgs.url = "github:nixos/nixpkgs/nixos-unstable";

  outputs =
    { nixpkgs, ... }:
    let
      forAllSystems =
        function:
        nixpkgs.lib.genAttrs nixpkgs.lib.systems.flakeExposed (
          system: function nixpkgs.legacyPackages.${system}
        );
    in
    {
      devShells = forAllSystems (pkgs: {
        default =
          let
            # Dalamud is a frequently moving target and will
            # need to be updated here frequently.
            #
            # TODO: Move to dedicate flake and automate.
            dalamud = pkgs.fetchzip {
              url = "https://raw.githubusercontent.com/goatcorp/dalamud-distrib/e88388b3eb5fdabd535c7409d6b63537eb82eca0/latest.zip";
              hash = "sha256-A8aEvAPONUGKqCIWUoUfZFbvHUkw+fAf/Bt5v2m28Qk=";
              stripRoot = false;
            };
          in
          pkgs.mkShell {
            packages = with pkgs; [
              dotnetCorePackages.sdk_10_0
              bashInteractive
            ];
            env = {
              DOTNET_ROOT = "${pkgs.dotnetCorePackages.sdk_10_0}";
              DALAMUD_HOME = "${dalamud}";
            };
          };
      });
    };
}
