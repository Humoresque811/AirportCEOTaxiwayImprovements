using Nodes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace AirportCEOTaxiwayImprovements.TaxiwayNodes;

internal class TaxiwayCenterBuilderSupplement : MonoBehaviour
{
    private TaxiwayCenterBuilder _taxiwayCenterBuilder;
    private TaxiwayNode _taxiwayNode;

    private bool[] ConnectorStatus => _taxiwayCenterBuilder.connectors;

    private bool _setUp = false;

    internal bool[] canConnectToPoints = new bool[8];

    internal void Init(TaxiwayCenterBuilder builder)
    {
        _taxiwayCenterBuilder = builder;
    }

    internal void SetUpNodeAndConnectors()
    {
        if (_taxiwayCenterBuilder == null)
        {
            return;
        }

        _taxiwayNode = Singleton<TaxiwayController>.Instance.GetNodeAtPosition(_taxiwayCenterBuilder.transform.position);

        UpdateConnectors();
        UpdateValidConnectionPoints();

        _setUp = true;
    }

    internal bool ShouldConnectTo(int index)
    {
        if (!ConnectorStatus[index])
        {
            return false;
        }

        TaxiwayCenterBuilderSupplement otherSupplement = GetSupplementFromIndex(index);
        if (otherSupplement == null)
        {
            AirportCEOTaxiwayImprovements.TILogger.LogError("Unable to get other supplement");
        }

        return otherSupplement.canConnectToPoints[TaxiwayNodeImageServer.GetOppositeIndex(index)];
    }

    internal bool HasConnectionAt(int index)
    {
        return ConnectorStatus[index];
    }


    private void UpdateConnectors()
    {
		ConnectorStatus[0] = _taxiwayNode.frontNodeConnector != null;
		ConnectorStatus[1] = _taxiwayNode.rightNodeConnector != null;
		ConnectorStatus[2] = _taxiwayNode.backNodeConnector != null;
		ConnectorStatus[3] = _taxiwayNode.leftNodeConnector != null;
		ConnectorStatus[4] = _taxiwayNode.topRightNodeConnector != null;
		ConnectorStatus[5] = _taxiwayNode.bottomRightNodeConnector != null;
		ConnectorStatus[6] = _taxiwayNode.bottomLeftNodeConnector != null;
		ConnectorStatus[7] = _taxiwayNode.topLeftNodeConnector != null;
    }

    private void UpdateValidConnectionPoints()
    {
        if (_setUp) // has been set up before, false it out
        {
            for (int i = 0; i < canConnectToPoints.Length; i++)
            {
                canConnectToPoints[i] = false;
            }
        }

        if (ConnectorStatus[0])
        {
            SetValidConnectionPointsTrue(6, 2, 5);
        }
        if (ConnectorStatus[1])
        {
            SetValidConnectionPointsTrue(7, 3, 6);
        }
        if (ConnectorStatus[2])
        {
            SetValidConnectionPointsTrue(7, 0, 4);
        }
        if (ConnectorStatus[3])
        {
            SetValidConnectionPointsTrue(4, 1, 5);
        }
        if (ConnectorStatus[4])
        {
            SetValidConnectionPointsTrue(7, 3, 6, 2, 5);
        }
        if (ConnectorStatus[5])
        {
            SetValidConnectionPointsTrue(6, 3, 7, 0, 4);
        }
        if (ConnectorStatus[6])
        {
            SetValidConnectionPointsTrue(7, 0, 4, 1, 5);
        }
        if (ConnectorStatus[7])
        {
            SetValidConnectionPointsTrue(4, 1, 5, 2, 6);
        }
    }

    private void SetValidConnectionPointsTrue(params int[] indices)
    {
        foreach (int index in indices)
        {
            canConnectToPoints[index] = true;
        }
    }

    private TaxiwayCenterBuilderSupplement GetSupplementFromIndex(int index)
    {
        (int, int) translation = TaxiwayNodeImageServer.GetTranslationFromIndex(index);
        Vector3 translationCopy = new Vector3(translation.Item1, translation.Item2, 0);
        if (GridController.Instance.TryGetStructureFromPosition<TaxiwayNodeModel>(_taxiwayCenterBuilder.transform.position + translationCopy * 4, Enums.StructureType.TaxiwayNode, out TaxiwayNodeModel taxiwayNodeModel))
        {
            if (taxiwayNodeModel.TryGetComponent<TaxiwayCenterBuilderSupplement>(out TaxiwayCenterBuilderSupplement supplement))
            {
                return supplement;
            }

            TaxiwayCenterBuilderSupplement sup = taxiwayNodeModel.gameObject.AddComponent<TaxiwayCenterBuilderSupplement>();
            sup.Init(taxiwayNodeModel.gameObject.GetComponent<TaxiwayCenterBuilder>());
            sup.SetUpNodeAndConnectors();
            return sup;
        }

        return null;
    }
}
